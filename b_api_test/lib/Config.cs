using b_api_test.test;
using Microsoft.Extensions.Configuration;

namespace b_api_test;

public static class Config
{
    private static readonly IConfiguration Configuration;

    static Config()
    {
        Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");

        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        Configuration = builder.Build();

        // Debug: Print all configuration values
        foreach (var config in Configuration.AsEnumerable())
        {
            Console.WriteLine($"Config: {config.Key} = {config.Value}");
        }
    }


    public static string BaseUrl1 => Configuration["ApiSettings:BaseUrl1"]
                                     ?? throw new InvalidOperationException("ApiSettings:BaseUrl1 is not set");

    public static string BaseUrl2 => Configuration["ApiSettings:BaseUrl2"]
                                     ?? throw new InvalidOperationException("ApiSettings:BaseUrl2 is not set");

    private static string token1 => Configuration["ApiSettings:Token1"] ?? "";
    private static string token2 => Configuration["ApiSettings:Token2"] ?? "";






    public static List<TestApiDetails> testSuite => TestData.CustomTestData;
    
    
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

public class TestApiDetails
{
    public string Endpoint { get; set; }
    public HttpMethod Method { get; set; }
    public object? Data { get; set; }
    public string TestName { get; set; }
}