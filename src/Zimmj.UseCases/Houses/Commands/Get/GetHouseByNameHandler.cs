using FluentResults;
using MediatR;
using Zimmj.Core.Houses;

namespace Zimmj.UseCases.Houses.Commands.Get;

public class GetHouseByNameHandler: IRequestHandler<GetHouseByNameCommand, Result<House>>
{
    private readonly IHouseRepository _houseRepository;

    public GetHouseByNameHandler(IHouseRepository houseRepository)
    {
        _houseRepository = houseRepository;
    }

    public Task<Result<House>> Handle(GetHouseByNameCommand request, CancellationToken cancellationToken)
    {
        return _houseRepository.GetAsync(request.Name);
    }
}