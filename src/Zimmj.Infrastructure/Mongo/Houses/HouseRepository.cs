using System.Collections.Immutable;
using FluentResults;
using Zimmj.Core.CrossCutting.ResultExtensions.Errors;
using Zimmj.Core.CrossCutting.Search;
using Zimmj.Core.Houses;
using Zimmj.Infrastructure.Mongo.Interfaces;
using Zimmj.Infrastructure.Mongo.Repositories;

namespace Zimmj.Infrastructure.Mongo.Houses;

public class HouseRepository : IHouseRepository
{
    private readonly IMongoRepository<HouseDocument, string> _houseRepository;

    public HouseRepository(IMongoRepository<HouseDocument, string> houseRepository)
    {
        _houseRepository = houseRepository;
    }

    public async Task<Result<House>> GetAsync(string name)
    {
        var houseDocument = await _houseRepository.GetAsync(name);
        if (houseDocument is null)
        {
            return Result.Fail<House>(new EntityNotFoundError(nameof(House)));
        }

        return Result.Ok(houseDocument.ToEntity());
    }

    public async Task<Result<SearchAnswer<House>>> FindAsync(HouseQuery houseQuery, Paginator paginator,
        SortBy<SortHouseBy> sortBy)
    {
        var search = houseQuery.ToExpression();
        var houses = await _houseRepository.FindAsync(
            search,
            MongoPaginator.FromEntity(paginator),
            sortBy.ToSortByFieldExpression(),
            sortBy.SortDirection
            );
        var count = await _houseRepository.CountAsync(search);
        return Result.Ok(
            new SearchAnswer<House>(houses.Select(document => document.ToEntity()).ToImmutableList(), count, paginator)
        );
    }

    public async Task<Result<List<House>>> GetAllDocumentsAsync()
    {
        var houses = await _houseRepository.GetAllDocumentsAsync();
        return Result.Ok(houses.Select(houseDocument => houseDocument.ToEntity()).ToList());
    }

    public async Task<Result> AddAsync(House entity)
    {
        await _houseRepository.AddAsync(HouseDocument.FromEntity(entity));
        return Result.Ok();
    }

    public async Task<Result> AddManyAsync(List<House> entity)
    {
        await _houseRepository.AddManyAsync(entity.Select(HouseDocument.FromEntity).ToList());
        return Result.Ok();
    }

    public async Task<Result> UpdateAsync(House entity)
    {
        await _houseRepository.UpdateAsync(HouseDocument.FromEntity(entity));
        return Result.Ok();
    }

    public async Task<Result> UpdateManyAsync(List<House> documents)
    {
        await _houseRepository.UpdateManyAsync(documents.Select(HouseDocument.FromEntity).ToList());
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(string id)
    {
        await _houseRepository.DeleteAsync(id);
        return Result.Ok();
    }
}