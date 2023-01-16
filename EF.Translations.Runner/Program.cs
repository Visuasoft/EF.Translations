// See https://aka.ms/new-console-template for more information

using System.Linq.Expressions;
using System.Threading.Channels;
using EF.Translations;
using EF.Translations.Usage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDbContext<BookDbContext>(options => options.UseInMemoryDatabase("Books"));

var provider = services.BuildServiceProvider();

var db = provider.GetService<BookDbContext>();
db.Database.EnsureCreated();

var category = await db.Categories.Include(c => c.Translations).FirstOrDefaultAsync();

var book = Book.Create("123456")
    .Title(new[]
    {
        "De verboden vrucht".Translated(),
        "The forbidden fruit".Translated(Languages.English)
    })
    .Publisher("Standaard Uitgeverij".Translated())
    .InCategory(category);

await db.Books.AddAsync(book);
await db.SaveChangesAsync();
db.ChangeTracker.Clear();

var selectLanguage = Languages.Dutch;

var nlBook = await db.Books.AsNoTracking()
    .Include(b => b.Translations)
    .Include(b => b.Category).ThenInclude(c => c.Translations)
    .WithTranslations(db.BookTranslations, selectLanguage)
    .Select(b => new BookResult(
        b.Entity.ISBN,
        b.Translation.Title ?? db.BookTranslations.First(x => x.CoreId == b.Entity.Id && x.Language == Languages.Default)
            .Title,
        b.Translation.Publisher ?? db.BookTranslations
            .First(x => x.CoreId == b.Entity.Id && x.Language == Languages.Default).Publisher,
        b.Entity.Category.ToString(selectLanguage)))
    .FirstOrDefaultAsync();


var select2Language = Languages.English;

var enBook = await db.Books.AsNoTracking()
    .Include(b => b.Translations)
    .Include(b => b.Category).ThenInclude(c => c.Translations)
    .WithTranslations(db.BookTranslations.AsNoTracking(), select2Language)
    .Select(b => new BookResult(
         b.Entity.ISBN,
         b.Translation.Title ?? b.DefaultTranslation.Title,
         b.Translation.Publisher ?? b.DefaultTranslation.Publisher,
        b.Entity.Category.ToString(select2Language)))
    .FirstOrDefaultAsync();


Console.WriteLine($"{nlBook.ISBN} {nlBook.Title} uitgegeven door {nlBook.Publisher} in categorie: {nlBook.Category}");
Console.WriteLine($"{enBook.ISBN} {enBook.Title} uitgegeven door {enBook.Publisher} in categorie: {enBook.Category}");

public class TranslatedQueryable<E, T> where E : EntityBase, IMultiLingualEntity<T>
    where T : EntityBase, IEntityTranslation<E>
{
    public E Entity { get; set; }
    public T Translation { get; set; }

    public T DefaultTranslation { get; set; }

}

public static class QueryExtensions
{
  
    public static IQueryable<TranslatedQueryable<E, T>> WithTranslations<E, T>(this IQueryable<E> entityQueryable,
        IQueryable<T> translations, string selectLanguage) where E : EntityBase, IMultiLingualEntity<T>
        where T : EntityBase, IEntityTranslation<E>
    {
        return entityQueryable.GroupJoin(translations.Where(b => b.Language == selectLanguage),
                b => b.Id,
                t => t.CoreId,
                (b, t) => new { Book = b, Translation = t }
            )
            .SelectMany(x=>x.Translation.DefaultIfEmpty(), (b,t)=> new{Book = b.Book, Translation = t})
            .GroupJoin(translations.Where(b=>b.Language == Languages.Default),
                b =>b.Book.Id,
                t =>t.CoreId,
                (b,t)=> new {Book = b.Book, Translation = b.Translation,  DefaultTranslation = t})
            .SelectMany(
                b => b.DefaultTranslation.DefaultIfEmpty(),
                (b, t) => new TranslatedQueryable<E, T>
                    { Entity = b.Book, Translation = b.Translation, DefaultTranslation = t}
            );
    }
}

public record BookResult(string ISBN, string Title, string Publisher, string Category);