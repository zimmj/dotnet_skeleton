using System.Linq.Expressions;

namespace Zimmj.Core.CrossCutting.Search;

public class SortBy<TEnum> where TEnum : Enum
{
    public TEnum SortByEnum { get;  }
    public SortDirection SortDirection { get; }
    
    public SortBy(TEnum sortByEnum, SortDirection sortDirection)
    {
        SortByEnum = sortByEnum;
        SortDirection = sortDirection;
    }
}