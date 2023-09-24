using Zimmj.Core.Houses;
using Zimmj.Presentation.CrossCutting.Dto;

namespace Zimmj.Presentation.Houses.Dto;

public class AddHouse : BaseDto<AddHouse, House>
{
    public string Name { get; init; } = string.Empty;
    public int Price { get; init; }
}