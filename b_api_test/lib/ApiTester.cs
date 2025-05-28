using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace b_api_test;

/// <summary>
/// Class for testing and comparing results from two different API service instances
/// </summary>
public class ApiTester
{
    private readonly ApiService _serviceA;
    private readonly ApiService _serviceB;
    
    /// <summary>
    /// Initializes a new instance of the ApiTester class with two configured API services
    /// </summary>
    /// <param name="configureServiceA">Action to configure the first API service</param>
    /// <param name="configureServiceB">Action to configure the second API service</param>
    public ApiTester(Action<ApiService> configureServiceA, Action<ApiService> configureServiceB)
    {
        _serviceA = new ApiService();
        _serviceB = new ApiService();
        
        configureServiceA(_serviceA);
        configureServiceB(_serviceB);
    }

    /// <summary>
    /// Tests a GET request to the specified endpoint with both API services and compares the results
    /// </summary>
    /// <param name="endpoint">The API endpoint to test</param>
    /// <returns>A tuple containing the results from both services and comparison metrics</returns>
    private async Task<ComparisonResult> TestGetAsync(string endpoint)
    {
        Console.WriteLine($"Testing GET request to endpoint: {endpoint}");
        
        var stopwatchA = Stopwatch.StartNew();
        var resultA = await _serviceA.GetAsync(endpoint);
        stopwatchA.Stop();
        
        var stopwatchB = Stopwatch.StartNew();
        var resultB = await _serviceB.GetAsync(endpoint);
        stopwatchB.Stop();
        
        var comparison = await CompareResults(resultA, resultB, stopwatchA.ElapsedMilliseconds, stopwatchB.ElapsedMilliseconds, null);
        DisplayComparisonResults(comparison);
        
        return comparison;
    }

    /// <summary>
    /// Tests a POST request to the specified endpoint with both API services and compares the results
    /// </summary>
    /// <param name="endpoint">The API endpoint to test</param>
    /// <param name="data">The data to send in the request body</param>
    /// <returns>A tuple containing the results from both services and comparison metrics</returns>
    private async Task<ComparisonResult>  TestPostAsync(string endpoint, object data)
    {
        Console.WriteLine($"Testing POST request to endpoint: {endpoint}");
        
        var stopwatchA = Stopwatch.StartNew();
        var resultA = await _serviceA.PostAsync(endpoint, data);
        stopwatchA.Stop();
        
        var stopwatchB = Stopwatch.StartNew();
        var resultB = await _serviceB.PostAsync(endpoint, data);
        stopwatchB.Stop();
        
        var comparison = await CompareResults(resultA, resultB, stopwatchA.ElapsedMilliseconds, stopwatchB.ElapsedMilliseconds, data);
        DisplayComparisonResults(comparison);
        
        return comparison;
    }
    
    /// <summary>
    /// Tests a PUT request to the specified endpoint with both API services and compares the results
    /// </summary>
    /// <param name="endpoint">The API endpoint to test</param>
    /// <param name="data">The data to send in the request body</param>
    /// <returns>A tuple containing the results from both services and comparison metrics</returns>
    private async  Task<ComparisonResult>  TestPutAsync(string endpoint, object data)
    {
        Console.WriteLine($"Testing PUT request to endpoint: {endpoint}");
        
        var stopwatchA = Stopwatch.StartNew();
        var resultA = await _serviceA.PutAsync(endpoint, data);
        stopwatchA.Stop();
        
        var stopwatchB = Stopwatch.StartNew();
        var resultB = await _serviceB.PutAsync(endpoint, data);
        stopwatchB.Stop();
        
        var comparison = await CompareResults(resultA, resultB, stopwatchA.ElapsedMilliseconds, stopwatchB.ElapsedMilliseconds,data);
        DisplayComparisonResults(comparison);
        
        return comparison;
    }

    /// <summary>
    /// Tests a PATCH request to the specified endpoint with both API services and compares the results
    /// </summary>
    /// <param name="endpoint">The API endpoint to test</param>
    /// <param name="data">The data to send in the request body</param>
    /// <returns>A tuple containing the results from both services and comparison metrics</returns>
    private async  Task<ComparisonResult>  TestPatchAsync(string endpoint, object data)
    {
        Console.WriteLine($"Testing PATCH request to endpoint: {endpoint}");
        
        var stopwatchA = Stopwatch.StartNew();
        var resultA = await _serviceA.PatchAsync(endpoint, data);
        stopwatchA.Stop();
        
        var stopwatchB = Stopwatch.StartNew();
        var resultB = await _serviceB.PatchAsync(endpoint, data);
        stopwatchB.Stop();
        
        var comparison = await CompareResults(resultA, resultB, stopwatchA.ElapsedMilliseconds, stopwatchB.ElapsedMilliseconds, data);
        DisplayComparisonResults(comparison);
        
        return comparison;
    }
    
    /// <summary>
    /// Tests a DELETE request to the specified endpoint with both API services and compares the results
    /// </summary>
    /// <param name="endpoint">The API endpoint to test</param>
    /// <returns>A tuple containing the results from both services and comparison metrics</returns>
    private async  Task<ComparisonResult>  TestDeleteAsync(string endpoint)
    {
        Console.WriteLine($"Testing DELETE request to endpoint: {endpoint}");
        
        var stopwatchA = Stopwatch.StartNew();
        var resultA = await _serviceA.DeleteAsync(endpoint);
        stopwatchA.Stop();
        
        var stopwatchB = Stopwatch.StartNew();
        var resultB = await _serviceB.DeleteAsync(endpoint);
        stopwatchB.Stop();
        
        var comparison = await CompareResults(resultA, resultB, stopwatchA.ElapsedMilliseconds, stopwatchB.ElapsedMilliseconds, null);
        DisplayComparisonResults(comparison);
        
        return  comparison;
    }
    
    /// <summary>
    /// Compares the results from both API services
    /// </summary>
    /// <param name="resultA">The result from the first API service</param>
    /// <param name="resultB">The result from the second API service</param>
    /// <param name="timeA">The time taken by the first API service in milliseconds</param>
    /// <param name="timeB">The time taken by the second API service in milliseconds</param>
    /// <returns>A comparison result object</returns>
private static async Task<ComparisonResult> CompareResults(HttpResponseMessage resultA, HttpResponseMessage resultB, long timeA, long timeB, object? requestData)
{
    var isJsonA = resultA.Content.Headers.ContentType?.MediaType?.Contains("json", StringComparison.OrdinalIgnoreCase) == true;
    var isJsonB = resultB.Content.Headers.ContentType?.MediaType?.Contains("json", StringComparison.OrdinalIgnoreCase) == true;

    var statusCodeA = (int)resultA.StatusCode;
    var statusCodeB = (int)resultB.StatusCode;

    var rawContentA = await resultA.Content.ReadAsStringAsync();
    var rawContentB = await resultB.Content.ReadAsStringAsync();

    string contentA = isJsonA ? MinifyJson(rawContentA) : "Need Manual Comparison";
    string contentB = isJsonB ? MinifyJson(rawContentB) : "Need Manual Comparison";

    var resultAContent = contentA.Length > 300 ? contentA.Substring(0, 300) + "..." : contentA;
    var resultBContent = contentB.Length > 300 ? contentB.Substring(0, 300) + "..." : contentB;

    var result = new ComparisonResult
    {
        TimeA = timeA,
        TimeB = timeB,
        TimeDifference = timeA - timeB,
        ResultsEqual = ComparisonStatus.Error,
        TestName = "Not specified",
        path = resultA.RequestMessage?.RequestUri?.PathAndQuery,
        method = resultA.RequestMessage?.Method.Method,
        requestData = requestData?.ToString() ?? "",
        ResultAStatusCode = statusCodeA,
        ResultBStatusCode = statusCodeB,
        ResultA = contentA,
        ResultB = contentB,
        ResultASummary = resultAContent,
        ResultBSummary = resultBContent
    };

    if (!isJsonA || !isJsonB)
    {
        result.Differences.Add("At least one response is not JSON; skipping comparison.");
        result.ResultsEqual = ComparisonStatus.Manual;
        return result;
    }

    bool isSuccessA = statusCodeA is >= 200 and <= 205;
    bool isSuccessB = statusCodeB is >= 200 and <= 205;
    if (!isSuccessA || !isSuccessB)
    {

        result.Differences.Add($"At least one response returned error status code: {statusCodeA}, {statusCodeB}");
        result.ResultsEqual = ComparisonStatus.Error;
        return result;
    }

    try
    {
        result.ResultsEqual = CompareJson(contentA, contentB) ? ComparisonStatus.Equal : ComparisonStatus.Different;

        if (result.ResultsEqual == ComparisonStatus.Different)
        {
            result.Differences.Add($"JSON content differs between responses");
        }
    }
    catch
    {

        result.Differences.Add($"Failed to parse JSON in one or both responses");
        result.ResultsEqual = ComparisonStatus.Error;
    }

    return result;
}

private static bool CompareJson(string contentA, string contentB)
{
    var jsonA = JsonSerializer.Deserialize<JsonElement>(contentA);
    var jsonB = JsonSerializer.Deserialize<JsonElement>(contentB);

    var serializedA = JsonSerializer.Serialize(jsonA, new JsonSerializerOptions { WriteIndented = false });
    var serializedB = JsonSerializer.Serialize(jsonB, new JsonSerializerOptions { WriteIndented = false });
    return serializedA.Trim() == serializedB.Trim();
}

// Utility function to minify JSON
private static string MinifyJson(string json)
{
    try
    {
        var parsed = JsonSerializer.Deserialize<JsonElement>(json);
        return JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = false });
    }
    catch
    {
        return json; // Return original if parsing fails
    }
}

    /// <summary>
    /// Displays the comparison results
    /// </summary>
    /// <param name="comparison">The comparison result</param>
    private static void DisplayComparisonResults(ComparisonResult comparison)
    {
        Console.WriteLine("Comparison Results:");
        Console.WriteLine($"Service A time: {comparison.TimeA} ms");
        Console.WriteLine($"Service B time: {comparison.TimeB} ms");
        Console.WriteLine($"Time difference: {comparison.TimeDifference} ms");
        Console.WriteLine($"Results equal: {comparison.ResultsEqual}");
        Console.WriteLine();
    }
    
    /// <summary>
    /// Runs a comprehensive test suite on both API services
    /// </summary>
    /// <returns>A dictionary of test results for each endpoint</returns>
    public async Task<Dictionary<string, ComparisonResult>> RunTestSuiteAsync(
        List<TestApiDetails> testSuits)
    {
        var results = new Dictionary<string, ComparisonResult>();
        
        foreach (var test in testSuits)
        {
            var testName = test.TestName;
            
            Console.WriteLine($"Running test: {testName}");
            
            try
            {
                var result = test.Method switch
                {
                    HttpMethod m when m == HttpMethod.Get => await TestGetAsync(test.Endpoint),
                    HttpMethod m when m == HttpMethod.Post => await TestPostAsync(test.Endpoint, test.Data),
                    HttpMethod m when m == HttpMethod.Put => await TestPutAsync(test.Endpoint, test.Data),
                    HttpMethod m when m == HttpMethod.Patch => await TestPatchAsync(test.Endpoint, test.Data),
                    HttpMethod m when m == HttpMethod.Delete => await TestDeleteAsync(test.Endpoint),
                    _ => throw new ArgumentException($"Unsupported HTTP method: {test.Method}")
                };
                
                // Set the test name in the comparison result
                result.TestName = testName;
                
                results.Add(testName, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test {testName} failed: {ex.Message}");
            }
        }
        
        return results;
    }
}

/// <summary>
/// Contains comparison metrics between two API requests
/// </summary>
public class ComparisonResult
{
    /// <summary>
    /// Name of the test
    /// </summary>
    public string TestName { get; set; } = string.Empty;
    
    /// <summary>
    /// Time taken by service A in milliseconds
    /// </summary>
    public long TimeA { get; set; }
    
    /// <summary>
    /// Time taken by service B in milliseconds
    /// </summary>
    public long TimeB { get; set; }
    
    /// <summary>
    /// Difference in time (TimeA - TimeB) in milliseconds
    /// </summary>
    public long TimeDifference { get; set; }
    
    /// <summary>
    /// Whether the results from both services are equal
    /// </summary>
    public ComparisonStatus ResultsEqual { get; set; }
    
    /// <summary>
    /// Status description of result A (null, has value, etc.)
    /// </summary>
    public int ResultAStatusCode { get; set; } = int.MinValue;
    
    /// <summary>
    /// Status description of result B (null, has value, etc.)
    /// </summary>
    public int ResultBStatusCode { get; set; } = int.MinValue;
    
    /// <summary>
    /// Summarized content of result A
    /// </summary>
    public string ResultASummary { get; set; } = string.Empty;
    
    /// <summary>
    /// Summarized content of result B
    /// </summary>
    public string ResultBSummary { get; set; } = string.Empty;
    
    /// <summary>
    /// Summarized content of result A
    /// </summary>
    public string ResultA { get; set; } = string.Empty;
    
    /// <summary>
    /// Summarized content of result B
    /// </summary>
    public string ResultB { get; set; } = string.Empty;
    
    public string? path { get; set; } = string.Empty;
    
    public string? method { get; set; } = string.Empty;
    
    public string? requestData { get; set; } = string.Empty;
    
    /// <summary>
    /// List of specific differences between results
    /// </summary>
    public List<string> Differences { get; set; } = new List<string>();
    
    
    public string ToString(bool includeDetails = false)
    {
        var equalityStatus = ResultsEqual.ToString();
        var timeDiffStr = TimeDifference >= 0 ? $"+{TimeDifference}" : TimeDifference.ToString();
        
        var basicInfo = $"{TestName} | {ResultAStatusCode} | {ResultBStatusCode} | {TimeA}ms | {TimeB}ms | {timeDiffStr}ms | {equalityStatus}";
        
        if (!includeDetails)
            return basicInfo;
            
        var details = new StringBuilder(basicInfo);
        details.AppendLine();
        details.AppendLine($"API Path: {path}");
        details.AppendLine($"Method: {method}");
        details.AppendLine();
        details.AppendLine("Request Data A:");
        details.AppendLine(requestData);
        details.AppendLine();
        details.AppendLine("Full Results:");
        details.AppendLine($"Response A: {ResultA}");
        details.AppendLine($"Response B: {ResultB}");
        
        if (Differences.Any())
        {
            details.AppendLine();
            details.AppendLine("Differences:");
            foreach (var diff in Differences)
            {
                details.AppendLine($"- {diff}");
            }
        }

        return details.ToString();
    }
}


// Create enum ComparisonStatus
public enum ComparisonStatus
{
    Equal,
    Different,
    Manual,
    Error
}

public static class ComparisonStatusExtensions
{
    public static System.Drawing.Color GetColor(this ComparisonStatus status)
    {
        return status switch
        {
            ComparisonStatus.Equal => System.Drawing.Color.Green,
            ComparisonStatus.Different => System.Drawing.Color.Red,
            ComparisonStatus.Manual => System.Drawing.Color.Orange,
            ComparisonStatus.Error => System.Drawing.Color.Gray,
            _ => System.Drawing.Color.Black
        };
    }
}