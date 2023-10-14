using Zimmj.Core.Houses;

namespace Zimmj.Integration.Tests.Houses;

public static class SeedHouseData
{

    public static readonly List<House> HousesSeed = new List<House>()
    {
        new ()
        {
            Name = "Cheap House",
            Price = 10,
        },
        new ()
        {
            Name = "Expensive House",
            Price = 1000000,
        },
        new ()
        {
            Name = "Test House",
            Price = 100,
        },
    };
}