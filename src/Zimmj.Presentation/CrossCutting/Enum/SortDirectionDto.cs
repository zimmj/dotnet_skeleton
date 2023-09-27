using Zimmj.Core.CrossCutting.Search;

namespace Zimmj.Presentation.CrossCutting.Enum;

public enum SortDirectionDto
{
    ASC,
    DESC
}

public static class Extensions
{
    public static SortDirection ToSortDirection(this SortDirectionDto sortDirectionDto)
    {
        return sortDirectionDto switch
        {
            SortDirectionDto.ASC => SortDirection.ASC,
            SortDirectionDto.DESC => SortDirection.DESC,
            _ => throw new ArgumentOutOfRangeException(nameof(sortDirectionDto), sortDirectionDto, null)
        };
    }
}