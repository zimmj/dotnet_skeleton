using FluentResults;
using MediatR;
using Zimmj.Core.Houses;

namespace Zimmj.UseCases.Houses.Commands.Add;

public class AddHouseHandler : IRequestHandler<AddHouseCommand, Result>
{
    private readonly IHouseRepository _houseRepository;

    public AddHouseHandler(
        IHouseRepository houseRepository)
    {
        _houseRepository = houseRepository;
    }
    
    public Task<Result> Handle(AddHouseCommand request, CancellationToken cancellationToken)
    {
        _houseRepository.AddAsync(request.House);
        return Task.FromResult(Result.Ok());
    }
}