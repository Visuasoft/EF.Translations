using Microsoft.EntityFrameworkCore;

namespace EF.Translations.Usage;

public class BookDbContext : DbContext
{
    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }

    public DbSet<BookTranslation> BookTranslations { get; set; }
    public DbSet<BookCategory> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BookTranslationEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BookCategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BookCategoryTranslationEntityTypeConfiguration());
    }
}