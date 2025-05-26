// See https://aka.ms/new-console-template for more information
using System.Net.Http.Json;
using b_api_test;

Console.WriteLine("API Service Example");



// Example of using ApiTester with two different API service configurations
var tester = new ApiTester(
    // Configure the first API service (Service A)
    serviceA => serviceA
        .SetBaseUrl("https://api.restful-api.dev/"),

        
    // Configure a second API service (Service B)
    serviceB => serviceB
        .SetBaseUrl("https://api.restful-api.dev/")
);

// Example of running tests (commented out since they would try to execute)
// await tester.TestGetAsync<WeatherData>("weather/current");
// await tester.TestPostAsync<WeatherData>("weather/record", weatherData);

// Example of running a test suite
var testSuite = new Dictionary<(string Endpoint, HttpMethod Method, object? Data), string>
{
    { ("/objects", HttpMethod.Get, null), "Test API" },
    { ("/objects?id=3&id=5&id=10", HttpMethod.Get, null), "Test API 2" },
};

// Execute the test suite
var results = await tester.RunTestSuiteAsync(testSuite);

Console.WriteLine("Test Suite Results:");

Console.WriteLine("Test Name | Status A | Status B | Time A | Time B | Time Difference | Results Equal");
Console.WriteLine("----------|----------|----------|--------|--------|----------------|-------------");

foreach (var result in results)
{
    Console.WriteLine(result.Value.Comparison.ToString(includeDetails: false));
}
// Get file paths for exports
var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

// Get the root directory (going up from bin/Debug/netX.X directory)
var currentDirectory = Directory.GetCurrentDirectory();
var rootDirectory = Path.GetFullPath(Path.Combine(currentDirectory, "../../../"));

var excelFilePath = Path.Combine(rootDirectory, $"api_results_{timestamp}.xlsx");

Console.WriteLine($"Root directory: {rootDirectory}");


// Export test results to Excel
if (ExcelExporter.ExportToExcel(results, excelFilePath))
{
    Console.WriteLine($"Excel report generated: {excelFilePath}");
}
else
{
    Console.WriteLine("Failed to export test results to Excel");
}

Console.WriteLine("\nAPI Service and ApiTester configured successfully!");