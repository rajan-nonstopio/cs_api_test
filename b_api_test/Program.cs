using b_api_test;

Console.WriteLine("API Service Example");

var tester = Config.CreateDefaultTester();
var testSuite = Config.testSuite;
// Execute the test suite
var results = await tester.RunTestSuiteAsync(testSuite);

Console.WriteLine("Test Suite Results:");

Console.WriteLine("Test Name | Status A | Status B | Time A | Time B | Time Difference | Results Equal");
Console.WriteLine("----------|----------|----------|--------|--------|----------------|-------------");

foreach (var result in results)
{
    Console.WriteLine(result.Value.ToString(includeDetails: true));
}
// Get file paths for exports
var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

// Get the root directory (going up from bin/Debug/netX.X directory)
var currentDirectory = Directory.GetCurrentDirectory();
var rootDirectory = Path.GetFullPath(Path.Combine(currentDirectory, "../../../"));

var excelFilePath = Path.Combine(rootDirectory, $"api_results_{timestamp}.xlsx");

Console.WriteLine($"Root directory: {rootDirectory}");


// Export test results to Excel
Console.WriteLine(ExcelExporter.ExportToExcel(results, Config.BaseUrl1, Config.BaseUrl2, excelFilePath)
    ? $"Excel report generated: {excelFilePath}"
    : "Failed to export test results to Excel");

Console.WriteLine("\nAPI Service and ApiTester configured successfully!");