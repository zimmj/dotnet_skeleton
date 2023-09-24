using FluentResults;

namespace Zimmj.Core.Houses;

public interface IHouseRepository
{
    Task<Result<House>> GetAsync(string name);
    Task<Result<List<House>>> FindAsync(HouseQuery query);
    Task<Result<List<House>>> GetAllDocumentsAsync();
    
    Task<Result> AddAsync(House entity);
    Task<Result> AddManyAsync(List<House> entity);

    Task<Result> UpdateAsync(House entity);
    
    Task<Result> UpdateManyAsync(List<House> documents);

    Task<Result> DeleteAsync(string id);
}