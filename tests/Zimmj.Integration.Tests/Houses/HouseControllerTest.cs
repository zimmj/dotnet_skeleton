using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using FluentAssertions;
using Zimmj.Integration.Tests.Fixtures;
using Zimmj.Presentation.Houses.Dto;

namespace Zimmj.Integration.Tests.Houses;

public class HouseControllerTest : IClassFixture<TestFixture<Program>>
{
    private readonly HttpClient _client;
    private readonly TestFixture<Program> _testFixture;

    public HouseControllerTest(
        TestFixture<Program> testFixture)
    {
        _testFixture = testFixture;
        testFixture.ConfigureTestOptions(options =>
        {
            options.ConfigureDatabase("DataBaseName");
            options.ConfigureWebHost(services =>
            {
                //Add Mocked services here for injection
            });
        });
       
        _client = testFixture.CreateClient();
    }


    [Fact]
    public async void GetHouse_WithWrongName_ShouldReturnNotFound()
    {
        // Arrange
        var name = "Wrong Name";
        
        // Act
        var response = await _client.GetAsync($"/api/houses/{name}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void GetHouse_WithCorrectName_ShouldReturnHouse()
    {
        // Arrange
        var name = "Test House";
        
        // Act
        var response = await _client.GetAsync($"/api/houses/{name}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var house = JsonSerializer.Deserialize<SimpleHouse>(
            await response.Content.ReadAsStringAsync(), options);
        
        house.Should().NotBeNull();
        house!.Name.Should().Be(name);
    }

    [Fact]
    public async void CreateHouse_ShouldReturnCreatedResponse()
    {
        // Arrange
        var addHouse = new AddHouse()
        {
            Name = "Test House",
            Price = 10
        };

        // Act
        var response = await _client.PostAsync(
            "/api/houses",
            GetJsonContent(addHouse)
        );

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var newHouse = await _client.GetAsync(response.Headers.Location);
        
        // Assert
        newHouse.StatusCode.Should().Be(HttpStatusCode.OK);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var house = JsonSerializer.Deserialize<SimpleHouse>(
            await newHouse.Content.ReadAsStringAsync(), options);

        house.Should().NotBeNull();
        house!.Name.Should().Be(addHouse.Name);
        house.Price.Should().Be(addHouse.Price);
    }

    [Fact]
    public async void Test1()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/api/houses");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private static StringContent GetJsonContent(Object content)
    {
        var json = JsonSerializer.Serialize(content);
        var stringContent = new StringContent(json);
        stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return stringContent;
    }
}