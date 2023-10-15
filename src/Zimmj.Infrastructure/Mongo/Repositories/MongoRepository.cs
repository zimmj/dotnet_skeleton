using System.Linq.Expressions;
using MongoDB.Driver;
using Zimmj.Infrastructure.Mongo.Interfaces;
using SortDirection = Zimmj.Core.CrossCutting.Search.SortDirection;

namespace Zimmj.Infrastructure.Mongo.Repositories;

internal class MongoRepository<TEntity, TIdentifiable> : IMongoRepository<TEntity, TIdentifiable>
    where TEntity : class, IIdentifiable<TIdentifiable>
    where TIdentifiable : notnull
{
    public MongoRepository(IMongoDatabase database, string collectionName) =>
        Collection = database.GetCollection<TEntity>(collectionName);

    private IMongoCollection<TEntity> Collection { get; }

    public Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
        => Collection.CountDocumentsAsync(predicate);

    public Task<TEntity?> GetAsync(TIdentifiable id)
        => GetAsync(entity => entity.Id.Equals(id));

    public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        => Collection.Find(predicate).SingleOrDefaultAsync();

    public Task<List<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        MongoPaginator paginator,
        Expression<Func<TEntity, object>> sortByField,
        SortDirection sortDirection)
    {
        var find = Collection.Find(predicate).Skip(paginator.Skip).Limit(paginator.Take);

        switch (sortDirection)
        {
            case SortDirection.ASC:
                return find.SortBy(sortByField).ToListAsync();
            case SortDirection.DESC:
                return find.SortByDescending(sortByField).ToListAsync();
            default:
                throw new ArgumentOutOfRangeException(nameof(sortDirection), sortDirection, null);
        }
    }

    public Task<List<TEntity>> GetAllDocumentsAsync()
        => Collection.Find(entity => true).ToListAsync();

    public Task AddAsync(TEntity entity)
        => Collection.InsertOneAsync(entity);


    public Task AddManyAsync(List<TEntity> entity)
        => Collection.InsertManyAsync(entity);

    public Task UpdateAsync(TEntity entity)
        => UpdateAsync(entity, e => e.Id.Equals(entity.Id));

    public Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate)
        => Collection.ReplaceOneAsync(predicate, entity);

    public Task UpdateManyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task UpdateManyAsync(Expression<Func<TEntity, bool>> predicate, UpdateDefinition<TEntity> update)
        => Collection.UpdateManyAsync(predicate, update);


    public Task UpdateManyAsync(List<TEntity> documents)
    {
        return Collection.BulkWriteAsync(
            documents.Select(
                document => new ReplaceOneModel<TEntity>(
                    new ExpressionFilterDefinition<TEntity>(entity => entity.Id.Equals(document.Id)), document)),
            new BulkWriteOptions() { IsOrdered = false });
    }

    public Task DeleteAsync(TIdentifiable id)
        => DeleteAsync(entity => entity.Id.Equals(id));

    public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        => Collection.DeleteOneAsync(predicate);

    public Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate)
        => Collection.DeleteManyAsync(predicate);
}