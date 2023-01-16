using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Translations;

public static class TranslationConfigurationExtensions
{
    public static void ConfigureTranslations<TEntity, TTranslation>(this EntityTypeBuilder<TEntity> entityBuilder)
        where TEntity : class, IMultiLingualEntity<TTranslation> where TTranslation : class, IEntityTranslation<TEntity>
    {
        entityBuilder.HasMany(f => f.Translations).WithOne(t => t.Core).HasForeignKey(t => t.CoreId);
    }
}

public interface IEntityTranslation<TEntity> : IEntityTranslation<TEntity, Guid>
{
    public Guid Id { get;  }
}

public interface IEntityTranslation
{
    string Language { get; set; }
}