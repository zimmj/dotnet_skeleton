using FluentResults;
using Zimmj.Core.CrossCutting.Search;

namespace Zimmj.Core.Houses;

public interface IHouseRepository
{
    Task<Result<House>> GetAsync(string name);
    Task<Result<SearchAnswer<House>>> FindAsync(HouseQuery query, Paginator paginator, SortBy<SortHouseBy> sortBy);
    
    Task<Result> AddAsync(House entity);
    Task<Result> AddManyAsync(List<House> entity);

    Task<Result> UpdateAsync(House entity);
    
    Task<Result> UpdateManyAsync(List<House> documents);

    Task<Result> DeleteAsync(string id);
}