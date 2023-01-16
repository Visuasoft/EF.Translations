namespace EF.Translations;

public interface IMultiLingualEntity<TTranslation> where TTranslation : class, IEntityTranslation
{
    TranslationCollection<TTranslation> Translations { get;  }
}