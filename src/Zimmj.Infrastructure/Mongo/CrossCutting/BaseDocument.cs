using Mapster;

namespace Zimmj.Infrastructure.Mongo.CrossCutting;

public class BaseDocument<TDocument, TEntity> : IRegister
    where TDocument : class, new()
    where TEntity : class, new()
{
    public TEntity ToEntity()
    {
        return this.Adapt<TEntity>();
    }

    public TEntity ToEntity(TEntity entity)
    {
        return (this as TDocument).Adapt(entity);
    }

    public static TDocument FromEntity(TEntity entity)
    {
        return entity.Adapt<TDocument>();
    }

    private TypeAdapterConfig Config { get; set; }

    public virtual void AddCustomMapping()
    {
    }

    protected TypeAdapterSetter<TDocument, TEntity> SetCustomMappings()
        => Config.ForType<TDocument, TEntity>();
    
    protected TypeAdapterSetter<TEntity, TDocument> SetCustomMappingsReverse()
        => Config.ForType<TEntity, TDocument>();

    public void Register(TypeAdapterConfig config)
    {
        Config = config;
        AddCustomMapping();
    }
}