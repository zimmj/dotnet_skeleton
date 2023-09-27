using Zimmj.Core.Houses;

namespace Zimmj.Presentation.Houses.Enum;

public enum SortHouseByDto
{
    Name,
    Price
}

public static class Extensions
{
    public static SortHouseBy ToSortHouseBy(this SortHouseByDto sortHouseByDto)
    {
        return sortHouseByDto switch
        {
            SortHouseByDto.Name => SortHouseBy.Name,
            SortHouseByDto.Price => SortHouseBy.Price,
            _ => throw new ArgumentOutOfRangeException(nameof(sortHouseByDto), sortHouseByDto, null)
        };
    }
}