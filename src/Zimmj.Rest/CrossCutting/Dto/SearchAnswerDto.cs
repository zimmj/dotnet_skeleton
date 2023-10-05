
namespace Zimmj.Rest.CrossCutting.Dto;

public class SearchAnswerDto<TDto>
{
    public long TotalCount { get; init; }
    public int Skip { get; init; }
    public int Take { get; init; }
    public IEnumerable<TDto> Items { get; init; } = Enumerable.Empty<TDto>();
}