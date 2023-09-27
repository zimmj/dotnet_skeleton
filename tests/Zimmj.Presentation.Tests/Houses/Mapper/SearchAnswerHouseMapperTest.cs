using Zimmj.Core.CrossCutting.Search;
using Zimmj.Core.Houses;
using Zimmj.Presentation.Houses.Mappings;

namespace Zimmj.Presentation.Tests.Houses.Mapper;

public class SearchAnswerHouseMapperTest
{
    [Theory, AutoData]
    public void ToEntity_ShouldMapAllFields(SearchAnswer<House> answer)
    {
        var mapped = SearchAnswerHouseMapper.FromEntity(answer);

        mapped.Should().BeEquivalentTo(answer, options => options.For(searchAnswer => searchAnswer.Items).Exclude(house => house.Id)
            .Excluding(searchAnswer => searchAnswer.Paginator));
    }
}