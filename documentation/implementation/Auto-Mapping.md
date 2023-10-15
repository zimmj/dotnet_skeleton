# Auto mapping

Mapping from one object to another object is one of the tidiest work in programming.
But it's as well one of the most important.
As each service might see the same object in different ways and we don't want to expose to many information to the outside world.

That's mean, almost all application have at least to mapping points:

- From third party object to domain object (Database to domain object)
- From domain object to outside object (Domain object to API object)

For this, programmer are using different implementation of automapper to reach this goal.
One of the most used is [AutoMapper](https://automapper.org/).

This has the disadvantage, that all mapping are added in profile on runtime.
The function does not exist on compile time.
And therefore can not be checked for type safety, or even if the mapping exist.

For this reason I decided to use [Mapster](https://github.com/MapsterMapper/Mapster).
It has the same functionality as AutoMapper, but the mapping are defined on compile time.

### My implementation of Mapster

I decided to have a quite simple but effective implementation of Mapster.

With a abstract base class, I can define the needed mapping.
Any object inherit from this base class, can define the mapping it needs.
With this, I can define the mapping on compile time and can be sure, that the mapping exist.

```csharp
public class BaseDto<TDto, TEntity> : IRegister
    where TDto : class, new()
    where TEntity : class, new()
{
    public TEntity ToEntity()
    {
        return this.Adapt<TEntity>();
    }

    public TEntity ToEntity(TEntity entity)
    {
        return (this as TDto).Adapt(entity);
    }

    public static TDto FromEntity(TEntity entity)
    {
        return entity.Adapt<TDto>();
    }

    private TypeAdapterConfig Config { get; set; }

    public virtual void AddCustomMapping()
    {
    }

    protected TypeAdapterSetter<TDto, TEntity> SetCustomMappings()
        => Config.ForType<TDto, TEntity>();
    
    protected TypeAdapterSetter<TEntity, TDto> SetCustomMappingsReverse()
        => Config.ForType<TEntity, TDto>();

    public void Register(TypeAdapterConfig config)
    {
        Config = config;
        AddCustomMapping();
    }
}
```

This base class as well allows costume mappings of field, which have not the same name in both object.
Or need a more complex mapping.

//todo check costume mapping and add example