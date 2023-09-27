using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Zimmj.Integration.Tests.Common;

public static class HttpResponseExtension
{
    public static T? Deserialize<T>(this HttpResponseMessage response)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<T>(
            response.Content.ReadAsStringAsync().Result, options);
    }
}