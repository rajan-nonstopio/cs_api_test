using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class OpenApiTestGenerator
{
    public static List<TestApiDetails> GenerateTestApiDetails(string jsonFilePath)
    {
        var testApiDetailsList = new List<TestApiDetails>();

        try
        {
            // Read the JSON file
            string jsonContent = File.ReadAllText(jsonFilePath);
            var openApiDoc = JObject.Parse(jsonContent);

            // Get the paths object
            var paths = openApiDoc["paths"] as JObject;
            if (paths == null) return testApiDetailsList;

            // Iterate through each path
            foreach (var path in paths)
            {
                string endpoint = path.Key;
                var pathItem = path.Value as JObject;

                if (pathItem == null) continue;

                // Check each HTTP method for this path
                foreach (var method in pathItem)
                {
                    string httpMethod = method.Key.ToUpper();
                    var operation = method.Value as JObject;

                    if (operation == null) continue;

                    // Get operation details
                    string summary = operation["summary"]?.ToString() ?? "";
                    bool isDeprecated = operation["deprecated"]?.ToObject<bool>() ?? false;

                    // Create test name
                    string testName = CreateTestName(httpMethod, endpoint, summary);

                    // Get sample data for POST requests
                    object? sampleData = null;
                    if (httpMethod == "POST")
                    {
                        sampleData = GenerateSampleData(operation, endpoint);
                    }

                    // Convert string method to HttpMethod
                    HttpMethod httpMethodEnum = httpMethod switch
                    {
                        "GET" => HttpMethod.Get,
                        "POST" => HttpMethod.Post,
                        "PUT" => HttpMethod.Put,
                        "DELETE" => HttpMethod.Delete,
                        "PATCH" => HttpMethod.Patch,
                        "HEAD" => HttpMethod.Head,
                        "OPTIONS" => HttpMethod.Options,
                        _ => HttpMethod.Get
                    };

                    // Create TestApiDetails object
                    var testApiDetails = new TestApiDetails
                    {
                        Endpoint = endpoint,
                        Method = httpMethodEnum,
                        Data = sampleData,
                        TestName = testName + (isDeprecated ? " (Deprecated)" : "")
                    };

                    testApiDetailsList.Add(testApiDetails);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating test API details: {ex.Message}");
        }

        return testApiDetailsList;
    }

    private static string CreateTestName(string method, string endpoint, string summary)
    {
        // Clean up the endpoint for test name
        string cleanEndpoint = endpoint
            .Replace("/api/", "")
            .Replace("/{", "_")
            .Replace("}", "")
            .Replace("/", "_")
            .Trim('_');

        // Use summary if available, otherwise create from endpoint
        if (!string.IsNullOrEmpty(summary))
        {
            return $"{method}_{cleanEndpoint}_{summary.Replace(" ", "_")}";
        }

        return $"{method}_{cleanEndpoint}";
    }

    private static object? GenerateSampleData(JObject operation, string endpoint)
    {
        try
        {
            var requestBody = operation["requestBody"] as JObject;
            if (requestBody == null) return null;

            var content = requestBody["content"] as JObject;
            if (content == null) return null;

            // Check for application/json content
            var jsonContent = content["application/json"] as JObject;
            if (jsonContent != null)
            {
                var schema = jsonContent["schema"] as JObject;
                if (schema != null)
                {
                    return GenerateSampleFromSchema(schema, endpoint);
                }
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    private static object? GenerateSampleFromSchema(JObject schema, string endpoint)
    {
        try
        {
            // Handle reference to components
            if (schema["$ref"] != null)
            {
                string refPath = schema["$ref"].ToString();
                if (refPath.Contains("AddReportDTO"))
                {
                    return new
                    {
                        caseId = "CASE123",
                        sampleId = 1001,
                        sampleName = "Sample_Test_001"
                    };
                }
            }

            // Handle direct schema definitions
            var properties = schema["properties"] as JObject;
            if (properties != null)
            {
                var sampleObject = new Dictionary<string, object>();

                foreach (var prop in properties)
                {
                    string propName = prop.Key;
                    var propSchema = prop.Value as JObject;

                    if (propSchema != null)
                    {
                        sampleObject[propName] = GenerateSampleValue(propName, propSchema);
                    }
                }

                return sampleObject;
            }

            // Handle string type (for simple request bodies)
            if (schema["type"]?.ToString() == "string")
            {
                return "sample_string_data";
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    private static object GenerateSampleValue(string propertyName, JObject propertySchema)
    {
        string type = propertySchema["type"]?.ToString() ?? "string";

        return type switch
        {
            "string" => propertyName.ToLower() switch
            {
                "caseid" => "CASE123",
                "samplename" => "Sample_Test_001",
                "userid" => "user123",
                _ => $"sample_{propertyName.ToLower()}"
            },
            "integer" => propertyName.ToLower() switch
            {
                "sampleid" => 1001,
                "id" => 1,
                _ => 100
            },
            "boolean" => true,
            _ => $"sample_{propertyName.ToLower()}"
        };
    }

    // Method to export the list to a C# file
    public static void ExportToFile(List<TestApiDetails> testApiDetailsList, string outputFilePath)
    {
        try
        {
            var csharpCode = GenerateCSharpCode(testApiDetailsList);
            File.WriteAllText(outputFilePath, csharpCode);
            Console.WriteLine($"Test API details exported to: {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exporting to file: {ex.Message}");
        }
    }

    private static string GenerateCSharpCode(List<TestApiDetails> testApiDetailsList)
    {
        var code = @"using System;
using System.Collections.Generic;
using System.Net.Http;

public class TestApiDetails
{
    public string Endpoint { get; set; }
    public HttpMethod Method { get; set; }
    public object? Data { get; set; }
    public string TestName { get; set; }
}

public static class GeneratedTestApiDetails
{
    public static List<TestApiDetails> GetTestApiDetailsList()
    {
        return new List<TestApiDetails>
        {
";

        foreach (var item in testApiDetailsList)
        {
            string dataString = "null";
            if (item.Data != null)
            {
                if (item.Data is string strData)
                {
                    dataString = $"\"{strData}\"";
                }
                else
                {
                    try
                    {
                        dataString =
                            $"@\"{JsonConvert.SerializeObject(item.Data, Formatting.None).Replace("\"", "\"\"")}\"";
                    }
                    catch
                    {
                        dataString = "null";
                    }
                }
            }

            string methodString = item.Method.Method switch
            {
                "GET" => "HttpMethod.Get",
                "POST" => "HttpMethod.Post",
                "PUT" => "HttpMethod.Put",
                "DELETE" => "HttpMethod.Delete",
                "PATCH" => "HttpMethod.Patch",
                "HEAD" => "HttpMethod.Head",
                "OPTIONS" => "HttpMethod.Options",
                _ => "HttpMethod.Get"
            };

            code += $@"            new TestApiDetails
            {{
                Endpoint = ""{item.Endpoint}"",
                Method = {methodString},
                Data = {dataString},
                TestName = ""{item.TestName}""
            }},
";
        }

        code += @"        };
    }
}";

        return code;
    }
}