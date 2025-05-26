// See https://aka.ms/new-console-template for more information
using System.Net.Http.Json;
using b_api_test;

Console.WriteLine("API Service Example");



// Example of using ApiTester with two different API service configurations
var tester = new ApiTester(
    // Configure the first API service (Service A)
    serviceA => serviceA
        .SetBaseUrl("https://jsonplaceholder.typicode.com"),

        
    // Configure a second API service (Service B)
    serviceB => serviceB
        .SetBaseUrl("https://jsonplaceholder.typicode.com")
);

// Example of running tests (commented out since they would try to execute)
// await tester.TestGetAsync<WeatherData>("weather/current");
// await tester.TestPostAsync<WeatherData>("weather/record", weatherData);

// Example of running a test suite
var testSuite = new Dictionary<(string Endpoint, HttpMethod Method, object? Data), string>
{
    { ("/todos/1", HttpMethod.Get, null), "Test API" },
};

// Commented out since it would try to execute
var results = await tester.RunTestSuiteAsync(testSuite);

Console.WriteLine("Test Suite Results:");

Console.WriteLine("Test Name | Result A | Result B | Time A | Time B | Time Difference | Results Equal");
Console.WriteLine("----------|------------|------------|------------|------------|------------|------------");

foreach (var result in results)
{
    Console.WriteLine(result.Value.Comparison.ToString(includeDetails:true));
}

Console.WriteLine("API Service and ApiTester configured successfully!");
Console.WriteLine("(Actual API calls are commented out to prevent execution)");

// Weather data model - moved after all top-level statements
