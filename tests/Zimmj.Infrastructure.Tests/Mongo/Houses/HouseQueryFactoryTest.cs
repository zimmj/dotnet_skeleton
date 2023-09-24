using LinqKit;
using Zimmj.Core.Houses;
using Zimmj.Infrastructure.Mongo.Houses;

namespace Zimmj.Infrastructure.Tests.Mongo.Houses;

public class HouseQueryFactoryTest
{

    [Theory]
    [InlineData(100, 0, 50, true)]
    [InlineData(100, 0, 99, true)]
    [InlineData(100, 0, 1, true)]
    [InlineData(100, 0, 150, false)]
    [InlineData(100, 50, 150, false)]
    [InlineData(100, 150, 200, false)]
    [InlineData(100, 0, 0, false)]
    [InlineData(100, 100, 100, false)]
    [InlineData(100, 100, 0, false)]
    public void HouseQueryToExpression_ShouldEvaluateCorrectly(int upperLimit, int lowerLimit, int housePrice, bool expected)
    {
        // Arrange
        var query = new HouseQuery()
        {
            UpperPriceLimit = upperLimit,
            LowerPriceLimit = lowerLimit
        };
        var house = new HouseDocument()
        {
            Id = "house",
            Name = "house",
            Price = housePrice
        };

        // Act
        var expression = query.ToExpression();
        var result = expression.Invoke(house);
        
        // Assert
        result.Should().Be(expected);

    }
}