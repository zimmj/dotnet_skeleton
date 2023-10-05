using Zimmj.Core.CrossCutting.Search;
using Zimmj.Core.Houses;
using Zimmj.Rest.CrossCutting.Dto;
using Zimmj.Rest.Houses.Dto;

namespace Zimmj.Rest.Houses.Mappings;

public static class SearchAnswerHouseMapper
{
    public static SearchAnswerDto<SimpleHouse> FromEntity(SearchAnswer<House> answer)
    {
        return new SearchAnswerDto<SimpleHouse>()
        {
            TotalCount = answer.TotalCount,
            Items = answer.Items.Select(SimpleHouse.FromEntity).ToList(),
            Skip = answer.Paginator.Skip,
            Take = answer.Paginator.Take
        };
    }
}