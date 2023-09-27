using Zimmj.Core.CrossCutting.Search;
using Zimmj.Core.Houses;
using Zimmj.Presentation.CrossCutting.Dto;
using Zimmj.Presentation.Houses.Dto;

namespace Zimmj.Presentation.Houses.Mappings;

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