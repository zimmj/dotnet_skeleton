using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using LinqKit;
using Zimmj.Core.Houses;

[assembly: InternalsVisibleTo("Zimmj.Infrastructure.Tests")]

namespace Zimmj.Infrastructure.Mongo.Houses;

internal static class HouseQueryFactory
{
    private static Expression<Func<HouseDocument, bool>> PriceLowerThanFilter(
        int price)
    {
        return PredicateBuilder.New<HouseDocument>()
            .And(house =>
                house.Price < price);
    }

    private static Expression<Func<HouseDocument, bool>> PriceHigherThanFilter(
        int price)
    {
        return PredicateBuilder.New<HouseDocument>()
            .And(house =>
                house.Price > price);
    }

    private static Expression<Func<HouseDocument, bool>> PriceIsInRangeFilter(
        int minPrice, int maxPrice)
    {
        return PredicateBuilder.New<HouseDocument>()
            .And(PriceHigherThanFilter(minPrice))
            .And(PriceLowerThanFilter(maxPrice));
    }

    internal static Expression<Func<HouseDocument, bool>> ToExpression(this HouseQuery houseQuery)
    {
        return PriceIsInRangeFilter(houseQuery.LowerPriceLimit, houseQuery.UpperPriceLimit);
    }
}