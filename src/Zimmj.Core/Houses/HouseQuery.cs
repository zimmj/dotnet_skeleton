namespace Zimmj.Core.Houses;

public class HouseQuery
{
    public int? UpperPriceLimit { get; init; }
    public int? LowerPriceLimit { get; init; }

    public HouseQuery()
    {
    }
    
    public HouseQuery(int? upperPriceLimit, int? lowerPriceLimit)
    {
        UpperPriceLimit = upperPriceLimit;
        LowerPriceLimit = lowerPriceLimit;
    }
}