using Zimmj.Integration.Tests.Common;
using Zimmj.Rest.Houses.Dto;

namespace Zimmj.Integration.Tests.Houses;

public static class SeedHouseData
{
    private static bool _once = false;
    public static async Task SeedData(HttpClient client)
    {
        if (_once)
            return;
        foreach (var addHouse in HousesSeed)
        {
            await client.PostAsync(
                "/api/houses", addHouse.ToJsonContent());
        }

        _once = true;
    }

    private static readonly List<AddHouse> HousesSeed = new List<AddHouse>()
    {
        new AddHouse()
        {
            Name = "Cheap House",
            Price = 10,
        },
        new AddHouse()
        {
            Name = "Expensive House",
            Price = 1000000,
        },
        new AddHouse()
        {
            Name = "Test House",
            Price = 100,
        },
    };
}