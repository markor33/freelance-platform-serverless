namespace Common.Layer.Headers;

public static class Headers
{
    public static Dictionary<string, string> CORS = new Dictionary<string, string>()
    {
        { "Content-Type", "application/json" },
        { "Access-Control-Allow-Origin", "*" },
        { "Access-Control-Allow-Headers", "Content-Type,Authorization" },
        { "Access-Control-Allow-Methods", "GET,POST,OPTIONS" }
    };
}
