using FluentResults;
using MediatR;
using Zimmj.Core.CrossCutting.Search;
using Zimmj.Core.Houses;

namespace Zimmj.Application.Houses.Commands.Get;

public class FilterHousesHandler : IRequestHandler<FilterHousesCommand, Result<SearchAnswer<House>>>
{
    private readonly IHouseRepository _houseRepository;

    public FilterHousesHandler(IHouseRepository houseRepository)
    {
        _houseRepository = houseRepository;
    }

    public Task<Result<SearchAnswer<House>>> Handle(FilterHousesCommand request, CancellationToken cancellationToken)
        => _houseRepository.FindAsync(request.HouseQuery, request.Paginator);
}