using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using LinqKit;
using Zimmj.Core.CrossCutting.Search;
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
        int? minPrice, int? maxPrice)
    {
        var expressionBuilder = PredicateBuilder.New<HouseDocument>(true);
        if (minPrice.HasValue)
            expressionBuilder.And(PriceHigherThanFilter(minPrice.Value));
        if (maxPrice.HasValue)
            expressionBuilder.And(PriceLowerThanFilter(maxPrice.Value));

        return expressionBuilder;
    }

    internal static Expression<Func<HouseDocument, bool>> ToExpression(this HouseQuery houseQuery)
    {
        return PriceIsInRangeFilter(houseQuery.LowerPriceLimit, houseQuery.UpperPriceLimit);
    }

    internal static Expression<Func<HouseDocument, object>> ToSortByFieldExpression(this SortBy<SortHouseBy> sortBy)
    {
        return sortBy.SortByEnum switch
        {
            SortHouseBy.Price => house => house.Price,
            SortHouseBy.Name => house => house.Name,
            _ => throw new ArgumentOutOfRangeException(nameof(sortBy.SortByEnum), sortBy.SortByEnum, "Unknown sort by enum")
        };
    }
}