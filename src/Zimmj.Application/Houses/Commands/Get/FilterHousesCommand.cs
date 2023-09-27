using FluentResults;
using MediatR;
using Zimmj.Core.CrossCutting.Search;
using Zimmj.Core.Houses;

namespace Zimmj.Application.Houses.Commands.Get;

public class FilterHousesCommand: IRequest<Result<SearchAnswer<House>>>
{
    public HouseQuery HouseQuery { get; }
    public Paginator Paginator { get; }

    public FilterHousesCommand(HouseQuery houseQuery, Paginator paginator)
    {
        HouseQuery = houseQuery;
        Paginator = paginator;
    }
}