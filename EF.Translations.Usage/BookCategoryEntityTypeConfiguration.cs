using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Translations.Usage;

public class BookCategoryEntityTypeConfiguration: IEntityTypeConfiguration<BookCategory>
{
    public void Configure(EntityTypeBuilder<BookCategory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
        builder.ConfigureTranslations<BookCategory,BookCategoryTranslation>();

        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<BookCategory> builder)
    {
        var categories = new[]
        {
            BookCategory.Create(new Guid("d70e9fda-e681-4135-851f-7c8169db0238"), 0),
            BookCategory.Create(new Guid("8502af03-d8ea-4e47-be35-7b73a93d7831"), 1),
            BookCategory.Create(new Guid("6ab8bb1e-199f-42d0-bb6a-5c34cc131134"), 2)
        };

        builder.HasData(categories);
    }
}

public class BookCategoryTranslationEntityTypeConfiguration: IEntityTypeConfiguration<BookCategoryTranslation>
{
    public void Configure(EntityTypeBuilder<BookCategoryTranslation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
        builder.Property(x => x.Category).HasMaxLength(250).IsRequired();

        SeedTranslations(builder);
    }

    private void SeedTranslations(EntityTypeBuilder<BookCategoryTranslation> builder)
    {
       var translations = new[]
       {
           new BookCategoryTranslation(){ Id = new Guid("c70eb670-5be0-446b-a4cb-7815f02efc4f"), CoreId = new Guid("d70e9fda-e681-4135-851f-7c8169db0238"), Language = Languages.Dutch, Category = "Thriller"},
           new BookCategoryTranslation(){ Id = new Guid("35e0ba03-6b30-4528-8cea-0c1ae26ad5dd"), CoreId = new Guid("8502af03-d8ea-4e47-be35-7b73a93d7831"), Language = Languages.Dutch, Category = "Fantasie"},
           new BookCategoryTranslation(){ Id = new Guid("3a0ac120-7752-4e8f-a6bd-85d231e55d11"), CoreId = new Guid("6ab8bb1e-199f-42d0-bb6a-5c34cc131134"), Language = Languages.Dutch, Category = "Liefde"},
           
           new BookCategoryTranslation(){ Id = new Guid("1b689bc3-d5c7-4e98-93fb-7e3b71d6ce36"), CoreId = new Guid("d70e9fda-e681-4135-851f-7c8169db0238"), Language = Languages.English, Category = "Suspense"},
           new BookCategoryTranslation(){ Id = new Guid("bb07a8d6-9019-4893-b585-0a9ab8ec049d"), CoreId = new Guid("8502af03-d8ea-4e47-be35-7b73a93d7831"), Language = Languages.English, Category = "Fantasy"},
           new BookCategoryTranslation(){ Id = new Guid("ec5e9ef3-b754-408d-81a4-26388a8bc0ba"), CoreId = new Guid("6ab8bb1e-199f-42d0-bb6a-5c34cc131134"), Language = Languages.English, Category = "Love stories"},
       };

       builder.HasData(translations);
       
    }
}