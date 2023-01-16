using System.Linq.Expressions;

namespace EF.Translations;

public static class Extensions
{
    public static TranslatedItem[] TranslatedItems<T>(this IMultiLingualEntity<T> entity, Expression<Func<T, object>> expression)
        where T : class, IEntityTranslation
    {
        return entity.Translations?.TranslatedValues(expression);
    }

    public static void SetTranslations<T>(this IMultiLingualEntity<T> entity, Expression<Func<T, object>> expression, TranslatedItem[] translations)
        where T : class, IEntityTranslation
    {
        entity.Translations?.AddTranslations(expression, translations);
    }
        
    public static void SetTranslation<T>(this IMultiLingualEntity<T> entity, Expression<Func<T, object>> expression, TranslatedItem translation)
        where T : class, IEntityTranslation
    {
        entity.Translations?.AddTranslation(expression, translation);
    }

    public static TranslatedItem Translated(this object value, string language = Languages.Default)
    {
        return new TranslatedItem
        {
            Language = language,
            Value = value
        };
    }
}