using System.Linq.Expressions;
using MongoDB.Driver;

namespace Zimmj.Infrastructure.Mongo.Interfaces;

public interface IMongoRepository<TEntity, in TIdentifiable>
    where TEntity : IIdentifiable<TIdentifiable>
    where TIdentifiable : notnull
{
    Task<TEntity?> GetAsync(TIdentifiable id);

    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);

    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetAllDocumentsAsync();
    
    Task AddAsync(TEntity entity);
    Task AddManyAsync(List<TEntity> entity);

    Task UpdateAsync(TEntity entity);
    
    Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate);
    Task UpdateManyAsync(Expression<Func<TEntity, bool>> predicate, UpdateDefinition<TEntity> update);
    Task UpdateManyAsync(List<TEntity> documents);

    Task DeleteAsync(TIdentifiable id);

    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate);
}