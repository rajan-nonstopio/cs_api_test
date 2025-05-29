namespace b_api_test.generator;

public static class Program
{
    public static void Main(string[] args)
    {
        // Generate test API details from the JSON file
        var jsonFilePath = "../../../test/sample.json"; // Update with your file path
        var testApiDetailsList = OpenApiTestGenerator.GenerateTestApiDetails(jsonFilePath);

        // Display generated details
        Console.WriteLine($"Generated {testApiDetailsList.Count} test API details:");
        foreach (var item in testApiDetailsList)
        {
            Console.WriteLine($"- {item.TestName}: {item.Method} {item.Endpoint}");
        }

        // Export to C# file
        string outputFilePath = "../../../test/GeneratedTestApiDetails.cs";
        OpenApiTestGenerator.ExportToFile(testApiDetailsList, outputFilePath);

        // You can also access the list directly
        var exportableList = testApiDetailsList;
        Console.WriteLine($"\nExportable list contains {exportableList.Count} items");
    }
}