using Zimmj.Core.Houses;
using Zimmj.Rest.CrossCutting.Dto;

namespace Zimmj.Rest.Houses.Dto;

public class AddHouse : BaseDto<AddHouse, House>
{
    public string Name { get; init; } = string.Empty;
    public int Price { get; init; }
}