using FluentResults;
using MediatR;
using Zimmj.Core.Houses;

namespace Zimmj.Application.Houses.Commands.Get;

public class GetHouseByNameCommand : IRequest<Result<House>>
{
    public string Name { get; }

    public GetHouseByNameCommand(string name)
    {
        Name = name;
    }
}