using FluentResults;
using MediatR;
using Zimmj.Core.Houses;

namespace Zimmj.UseCases.Houses.Commands.Add;

public class AddHouseCommand : IRequest<Result>
{
    public AddHouseCommand(House house)
    {
        House = house;
    }
    public House House { get; }
}