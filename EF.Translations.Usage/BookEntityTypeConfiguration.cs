using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Translations.Usage;

public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
        builder.Property(x => x.ISBN);
        builder.ConfigureTranslations<Book,BookTranslation>();
        builder.HasOne(x => x.Category).WithOne();
    }
}

public class BookTranslationEntityTypeConfiguration : IEntityTypeConfiguration<BookTranslation>
{
    public void Configure(EntityTypeBuilder<BookTranslation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
        builder.Property(x => x.Title).HasMaxLength(500).IsRequired(false);
        builder.Property(x => x.Publisher).HasMaxLength(500).IsRequired(false);
        
    }
}