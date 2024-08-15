using System.Text.Json;

namespace Common.Layer.JsonOptions;

public static class JsonOptions
{
    public static JsonSerializerOptions Options => new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
