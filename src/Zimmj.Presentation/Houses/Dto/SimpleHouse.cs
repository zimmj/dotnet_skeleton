using System.Text.Json.Serialization;
using Zimmj.Core.Houses;
using Zimmj.Presentation.CrossCutting.Dto;

namespace Zimmj.Presentation.Houses.Dto;

public class SimpleHouse : BaseDto<SimpleHouse, House>
{
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
}