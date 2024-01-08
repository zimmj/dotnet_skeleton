using FluentResults;
using MediatR;
using Zimmj.Core.CrossCutting.Search;
using Zimmj.Core.Houses;

namespace Zimmj.UseCases.Houses.Commands.Get;

public class FilterHousesCommand: IRequest<Result<SearchAnswer<House>>>
{
    public HouseQuery HouseQuery { get; }
    public Paginator Paginator { get; }
    public SortBy<SortHouseBy> SortBy {get; }

    public FilterHousesCommand(HouseQuery houseQuery, Paginator paginator, SortBy<SortHouseBy> sortBy)
    {
        HouseQuery = houseQuery;
        Paginator = paginator;
        SortBy = sortBy;
    }
}