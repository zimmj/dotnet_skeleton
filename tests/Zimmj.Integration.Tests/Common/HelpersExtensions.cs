using System.Net.Http.Headers;
using System.Text.Json;
using Zimmj.Presentation.Houses.Dto;

namespace Zimmj.Integration.Tests.Common;

public static class HelpersExtensions
{
    public static StringContent ToJsonContent(this AddHouse simpleHouse)
    {
        var json = JsonSerializer.Serialize(simpleHouse);
        var stringContent = new StringContent(json);
        stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return stringContent;
    }
}