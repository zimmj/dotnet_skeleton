using System.Text.Json.Serialization;
using Zimmj.Core.Houses;
using Zimmj.Rest.CrossCutting.Dto;

namespace Zimmj.Rest.Houses.Dto;

public class SimpleHouse : BaseDto<SimpleHouse, House>
{
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
}