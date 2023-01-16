namespace EF.Translations.Usage;

public class BookCategory : EntityBase, IMultiLingualEntity<BookCategoryTranslation>, ITranslatedToString
{
    public int SortOrder { get; private set; }
    public TranslationCollection<BookCategoryTranslation> Translations { get; private set; } = new();

    private BookCategory()
    {
    }

    protected BookCategory(Guid id, int sortOrder)
    {
        this.Id = id;
        sortOrder = sortOrder;
    }

    public static BookCategory Create(Guid id, int sortOrder) =>
        new(id, sortOrder);


    public string ToString(string language = Languages.Default)
    {
        return Translations[language]?.Category;
    }
}

public class BookCategoryTranslation : EntityBase, IEntityTranslation<BookCategory>
{
    public string Language { get; set; }
    public BookCategory Core { get; }
    public Guid CoreId { get; set; }
    public string Category { get; set; }
}