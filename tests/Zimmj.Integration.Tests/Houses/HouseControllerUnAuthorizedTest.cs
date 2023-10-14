using System.Net.Http.Json;
using Zimmj.Integration.Tests.Fixtures;
using Zimmj.Rest.Houses.Dto;

namespace Zimmj.Integration.Tests.Houses;

public class HouseControllerUnAuthorizedTest : IClassFixture<TestFixture<Program>>
{
    private readonly HttpClient _client;

    public HouseControllerUnAuthorizedTest(
        TestFixture<Program> testFixture)
    {
        testFixture.ConfigureTestOptions(options =>
        {
            options.ConfigureDatabase("HouseControllerTest");
            options.ConfigureWebHost(services =>
            {
                //Add Mocked services here for injection
            });
        });

        _client = testFixture.CreateClient();
        // As we use singleton client, all calls will be authenticated
    }

    [Fact]
    public async void GetHouse_WithOutToken_ShouldReturnUnAuthorized()
    {
        // Arrange
        var name = "Test House";
        // Act
        var response = await _client.GetAsync($"/api/houses/{name}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async void PostHouse_WithOutToken_ShouldReturnUnAuthorized()
    {
        // Arrange
        var addHouse = new AddHouse()
        {
            Name = "New Test House",
            Price = 10
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/houses", addHouse);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async void SearchForHouse_WithOutToken_ShouldReturnUnAuthorized()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync($"/api/houses");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}