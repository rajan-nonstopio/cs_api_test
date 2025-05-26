namespace b_api_test;

public static class Config
{
    public static string BaseUrl1 => "https://api-qa.baylorgenetics.com";
    public static string BaseUrl2 => "https://sample-management-api-qa.baylorgenetics.com";

    private static string token1 => "";

    private static string token2 => "";
    
    public static Dictionary<(string Endpoint, HttpMethod Method, object? Data), string> DefaultTestSuite => new()
    {
        { ("/api/clientTestCodes/custom-panel?hospitalCode=NISC&preCarveOutTestCode=24001", HttpMethod.Get, null), "Client Custom panels" },
        { ("/api/clientTestCodes/code/24001", HttpMethod.Get, null), "Client Test code" },
        { ("/api/clientTestCodes?clientTestcode=NISC-24001-P2-1", HttpMethod.Get, null), "Get Client Test codes" },
    };
    
    public static ApiTester CreateDefaultTester()
    {
        return new ApiTester(
            // Configure the first API service (Service A)
            serviceA => serviceA
                .SetBaseUrl(BaseUrl1)
                .AddHeader("accept", "application/json"),
            
            // Configure a second API service (Service B)
            serviceB => serviceB
                .SetBaseUrl(BaseUrl2)
                .AddHeader("accept", "application/json")
        );
    }
}