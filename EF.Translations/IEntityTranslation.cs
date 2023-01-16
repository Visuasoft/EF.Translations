namespace EF.Translations;

public interface IEntityTranslation<TEntity, TPrimaryKeyOfMultiLingualEntity> : IEntityTranslation
{
    TEntity Core { get;  }
    TPrimaryKeyOfMultiLingualEntity CoreId { get;  }
}