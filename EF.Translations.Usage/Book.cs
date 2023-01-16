using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace EF.Translations.Usage;

public class Book : EntityBase, IMultiLingualEntity<BookTranslation>, ITranslatedToString
{
    public string ISBN { get; private set; }

    private Book()
    {
    }

    protected Book(string isbn)
    {
        ISBN = isbn;
    }

    public static Book Create(string isbn) => new(isbn);
    
    public TranslationCollection<BookTranslation> Translations { get; set; } = new();

    public string Title(string language = Languages.Default) => Translations[language]?.Title;

    public Book Title(TranslatedItem translation)
    {
        this.SetTranslation(b => b.Title, translation);
        return this;
    }

    public Book Title(TranslatedItem[] translations)
    {
        this.SetTranslations(b => b.Title, translations);
        return this;
    }

    public string Publisher(string language = Languages.Default) => Translations[language]?.Publisher;

    public Book Publisher(TranslatedItem translation)
    {
        this.SetTranslation(b => b.Publisher, translation);
        return this;
    }

    public Book Publisher(TranslatedItem[] translations)
    {
        this.SetTranslations(b => b.Publisher, translations);
        return this;
    }

    public Guid BookCategoryId { get; set; }
    public virtual BookCategory Category { get; set; }

    public Book InCategory(BookCategory category)
    {
        this.BookCategoryId = category.Id;
        this.Category = category;
        return this;
    }

    public string GetCategory(string language = Languages.Default) => this.Category.ToString(language);

    public string ToString(string language = Languages.Default)
    {
        return $"{ISBN} - {Title(language)}";
    }
}

public class BookTranslation : EntityBase, IEntityTranslation<Book>
{
    public string Publisher { get; private set; }
    public string Title { get; private set; }
    public string Language { get; set; }
    
    public Book Core { get; private set; }
    public Guid CoreId { get; private set; }
}