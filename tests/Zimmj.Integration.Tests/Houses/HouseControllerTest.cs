using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using FluentAssertions;
using Zimmj.Integration.Tests.Common;
using Zimmj.Integration.Tests.Fixtures;
using Zimmj.Presentation.CrossCutting.Dto;
using Zimmj.Presentation.CrossCutting.Enum;
using Zimmj.Presentation.Houses.Dto;
using Zimmj.Presentation.Houses.Enum;

namespace Zimmj.Integration.Tests.Houses;

public class HouseControllerTest : IClassFixture<TestFixture<Program>>
{
    private readonly HttpClient _client;

    public HouseControllerTest(
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
        SeedHouseData.SeedData(_client).Wait();
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
            Name = "New Test House",
            Price = 10
        };

        // Act
        var response = await _client.PostAsync(
            "/api/houses",
            addHouse.ToJsonContent()
        );

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var newHouse = await _client.GetAsync(response.Headers.Location);
        
        // Assert
        newHouse.StatusCode.Should().Be(HttpStatusCode.OK);
        var house = newHouse.Deserialize<SimpleHouse>();

        house.Should().NotBeNull();
        house!.Name.Should().Be(addHouse.Name);
        house.Price.Should().Be(addHouse.Price);
    }

    [Fact]
    public async void FilterHouses_WithNonQuery_ShouldReturnAllHouses()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/api/houses");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var housesAnswer = response.Deserialize<SearchAnswerDto<SimpleHouse>>();
        
        housesAnswer.Should().NotBeNull();
        housesAnswer!.Items.Should().NotBeEmpty();
        housesAnswer.Items.Should().HaveCount((int)housesAnswer.TotalCount);
    }
    
    [Theory]
    [InlineData(SortHouseByDto.Price, SortDirectionDto.ASC, "Cheap House")]
    [InlineData(SortHouseByDto.Price, SortDirectionDto.DESC, "Expensive House")]
    [InlineData(SortHouseByDto.Name, SortDirectionDto.ASC, "Cheap House")]
    [InlineData(SortHouseByDto.Name, SortDirectionDto.DESC, "Test House")]
    public async void FilterHouses_WithSort_ShouldReturnSortedHouses(SortHouseByDto sortBy, SortDirectionDto direction, string nameOfFirstHouse)
    {
        // Arrange

        // Act
        var response = await _client.GetAsync($"/api/houses?sortBy={sortBy}&sortDirection={direction}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var housesAnswer = response.Deserialize<SearchAnswerDto<SimpleHouse>>();
        
        housesAnswer.Should().NotBeNull();
        housesAnswer!.Items.Should().NotBeEmpty();
        housesAnswer.Items.Should().HaveCount((int)housesAnswer.TotalCount);
        housesAnswer.Items.First().Name.Should().Be(nameOfFirstHouse);
    }

    [Theory]
    [InlineData(100, 0, 1)]
    [InlineData(1000140, 0, 3)]
    [InlineData(1000140, 300, 1)]
    public async void FilterHouse_WithDifferentLimits_ShouldReturnDifferentHouses(int? upperLimit, int? lowerLimit, int count)
    {
        // Arrange
        
        // Act
        var response = await _client.GetAsync($"/api/houses?upperPrice={upperLimit}&lowerPrice={lowerLimit}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var housesAnswer = response.Deserialize<SearchAnswerDto<SimpleHouse>>();
        
        housesAnswer.Should().NotBeNull();
        housesAnswer!.Items.Should().NotBeEmpty();
        housesAnswer.Items.Should().HaveCount(count);
        housesAnswer.Items.Should().OnlyContain(house => house.Price <= upperLimit && house.Price >= lowerLimit);
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