using Zimmj.Integration.Tests.Common;
using Zimmj.Presentation.Houses.Dto;

namespace Zimmj.Integration.Tests.Houses;

public static class SeedHouseData
{
    private static bool Once = false;
    public static async Task SeedData(HttpClient client)
    {
        if (Once)
            return;
        foreach (var addHouse in HousesSeed)
        {
            await client.PostAsync(
                "/api/houses", addHouse.ToJsonContent());
        }

        Once = true;
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