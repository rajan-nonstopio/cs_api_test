namespace b_api_test;

public static class Config
{
    public static string BaseUrl1 => "https://api.restful-api.dev/";
    public static string BaseUrl2 => "https://api.restful-api.dev/";
    
    public static Dictionary<(string Endpoint, HttpMethod Method, object? Data), string> DefaultTestSuite => new()
    {
        { ("/objects", HttpMethod.Get, null), "Test API" },
        { ("/objects?id=3&id=5&id=10", HttpMethod.Get, null), "Test API 2" },
    };
    
    public static ApiTester CreateDefaultTester()
    {
        return new ApiTester(
            // Configure the first API service (Service A)
            serviceA => serviceA
                .SetBaseUrl(BaseUrl1),
            
            // Configure a second API service (Service B)
            serviceB => serviceB
                .SetBaseUrl(BaseUrl2)
        );
    }
}