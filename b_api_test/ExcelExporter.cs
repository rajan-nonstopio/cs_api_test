using System.IO;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace b_api_test;

/// <summary>
/// Provides functionality to export test results to Excel format
/// </summary>
public static class ExcelExporter
{
    /// <summary>
    /// Exports test results to a well-formatted Excel file
    /// </summary>
    /// <typeparam name="T">Type of the test results</typeparam>
    /// <param name="results">Dictionary containing test results</param>
    /// <param name="filePath">Path to save the Excel file</param>
    /// <returns>True if the export was successful, false otherwise</returns>
    public static bool ExportToExcel(
        Dictionary<string, ComparisonResult> results,
        string baseUrl1,
        string baseUrl2,
        string filePath)
    {
        try
        {
            // Set license context for EPPlus (commercial use requires license)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using var package = new ExcelPackage();
            
            // Create a summary worksheet
            CreateSummarySheet(package, results, baseUrl1, baseUrl2);
            
            // Create a detailed results worksheet
            CreateDetailedResultsSheet(package, results);
            
            // Save to file
            package.SaveAs(new FileInfo(filePath));
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exporting to Excel: {ex.Message}");
            return false;
        }
    }
    
    /// <summary>
    /// Creates a summary sheet with an overview of test results
    /// </summary>
    private static void CreateSummarySheet(
        ExcelPackage package, 
        Dictionary<string, ComparisonResult> results,
        string baseUrl1,
        string baseUrl2
        )
    {
        var worksheet = package.Workbook.Worksheets.Add("Summary");
        
        // Title and timestamp
        worksheet.Cells[1, 1].Value = "API Test Results Summary";
        worksheet.Cells[1, 1].Style.Font.Size = 16;
        worksheet.Cells[1, 1].Style.Font.Bold = true;
        
        worksheet.Cells[2, 1].Value = "Generated:";
        worksheet.Cells[2, 2].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Base URLs
        worksheet.Cells[3, 1].Value = "Service A URL:";
        worksheet.Cells[3, 2].Value = baseUrl1;
        worksheet.Cells[4, 1].Value = "Service B URL:";
        worksheet.Cells[4, 2].Value = baseUrl2;
        
        // Statistics
        worksheet.Cells[6, 1].Value = "Statistics";
        worksheet.Cells[6, 1].Style.Font.Bold = true;
        
        worksheet.Cells[7, 1].Value = "Total Tests:";
        worksheet.Cells[7, 2].Value = results.Count;
        
        var successCount = results.Count(r => r.Value.ResultsEqual);
        var failureCount = results.Count - successCount;
        
        worksheet.Cells[8, 1].Value = "Successful Tests:";
        worksheet.Cells[8, 2].Value = successCount;
        
        worksheet.Cells[9, 1].Value = "Failed Tests:";
        worksheet.Cells[9, 2].Value = failureCount;
        
        worksheet.Cells[10, 1].Value = "Success Rate:";
        worksheet.Cells[10, 2].Value = results.Count > 0 
            ? (double)successCount / results.Count 
            : 0;
        worksheet.Cells[10, 2].Style.Numberformat.Format = "0.00%";
        
        // Average timings
        if (results.Any())
        {
            var avgTimeA = results.Average(r => r.Value.TimeA);
            var avgTimeB = results.Average(r => r.Value.TimeB);
            var avgTimeDiff = results.Average(r => r.Value.TimeDifference);
            
            worksheet.Cells[12, 1].Value = "Average Response Times";
            worksheet.Cells[12, 1].Style.Font.Bold = true;
            
            worksheet.Cells[13, 1].Value = "Service A:";
            worksheet.Cells[13, 2].Value = avgTimeA;
            worksheet.Cells[13, 2].Style.Numberformat.Format = "0.00 \"ms\"";
            
            worksheet.Cells[14, 1].Value = "Service B:";
            worksheet.Cells[14, 2].Value = avgTimeB;
            worksheet.Cells[14, 2].Style.Numberformat.Format = "0.00 \"ms\"";
            
            worksheet.Cells[15, 1].Value = "Difference (A-B):";
            worksheet.Cells[15, 2].Value = avgTimeDiff;
            worksheet.Cells[15, 2].Style.Numberformat.Format = "0.00 \"ms\"";
            
            // Set color based on difference (green if A is faster, red if B it is faster)
            if (avgTimeDiff < 0)
            {
                worksheet.Cells[15, 2].Style.Font.Color.SetColor(Color.Green);
            }
            else if (avgTimeDiff > 0)
            {
                worksheet.Cells[15, 2].Style.Font.Color.SetColor(Color.Red);
            }
        }
        
        // Test result overview table
        worksheet.Cells[17, 1].Value = "Test Results Overview";
        worksheet.Cells[17, 1].Style.Font.Bold = true;
        
        // Headers
        var headers = new[] { "Test Name", "API Path", "Status", "Status A", "Status B", "Time A (ms)", "Time B (ms)", "Diff (ms)" };
        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cells[18, i + 1].Value = headers[i];
            worksheet.Cells[18, i + 1].Style.Font.Bold = true;
            worksheet.Cells[18, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[18, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[18, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }
        
        // Data rows
        int row = 19;
        foreach (var (testName,  comparison) in results)
        {
            // Extract an API path from the test name
            
            worksheet.Cells[row, 1].Value = testName;
            worksheet.Cells[row, 2].Value = comparison.path;
            
            worksheet.Cells[row, 3].Value = comparison.ResultsEqual ? "Success" : "Failure";
            worksheet.Cells[row, 3].Style.Font.Color.SetColor(comparison.ResultsEqual ? Color.Green : Color.Red);
            
            worksheet.Cells[row, 4].Value = comparison.ResultAStatusCode;
            FormatStatusCodeCell(worksheet.Cells[row, 4], comparison.ResultAStatusCode);
            
            worksheet.Cells[row, 5].Value = comparison.ResultBStatusCode;
            FormatStatusCodeCell(worksheet.Cells[row, 5], comparison.ResultBStatusCode);
            
            worksheet.Cells[row, 6].Value = comparison.TimeA;
            worksheet.Cells[row, 7].Value = comparison.TimeB;
            
            worksheet.Cells[row, 8].Value = comparison.TimeDifference;
            if (comparison.TimeDifference < 0)
            {
                worksheet.Cells[row, 8].Style.Font.Color.SetColor(Color.Green);
            }
            else if (comparison.TimeDifference > 0)
            {
                worksheet.Cells[row, 8].Style.Font.Color.SetColor(Color.Red);
            }
            
            // Add border to all cells in the row
            for (int i = 1; i <= headers.Length; i++)
            {
                worksheet.Cells[row, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            
            row++;
        }
        
        // Auto-fit columns
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
    }
    
    /// <summary>
    /// Creates a detailed results sheet with all test information
    /// </summary>
    private static void CreateDetailedResultsSheet(
        ExcelPackage package, 
        Dictionary<string, ComparisonResult> results
    ){
        var worksheet = package.Workbook.Worksheets.Add("Detailed Results");
        ;
        // Title    
        worksheet.Cells[1, 1].Value = "API Test Detailed Results";
        worksheet.Cells[1, 1].Style.Font.Size = 16;
        worksheet.Cells[1, 1].Style.Font.Bold = true;
// Details
        // Headers
        var headers = new[] 
        { 
            "Test Name", 
            "API Path",
            "Method",
            "Equal",
            "Status A", 
            "Status B", 
            "Time A (ms)", 
            "Time B (ms)", 
            "Diff (ms)",
            "Request Body",
            "Response Body A",
            "Response Body B",
            "Details"
        };
        
        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cells[3, i + 1].Value = headers[i];
            worksheet.Cells[3, i + 1].Style.Font.Bold = true;
            worksheet.Cells[3, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[3, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[3, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }
        
        // Data rows
       var row = 4;
        foreach (var result in results)
        {
            // Extract an API path from the test name if possible
            
            var testName = result.Key;
            var comparison = result.Value;
                
            worksheet.Cells[row, 1].Value = testName;
            worksheet.Cells[row, 2].Value = comparison.path;
            worksheet.Cells[row, 3].Value = comparison.method;
            
            // Equal status with color coding
            worksheet.Cells[row, 4].Value = comparison.ResultsEqual;
            worksheet.Cells[row, 4].Style.Font.Color.SetColor(comparison.ResultsEqual ? Color.Green : Color.Red);
            
            // Status codes
            worksheet.Cells[row, 5].Value = comparison.ResultAStatusCode;
            FormatStatusCodeCell(worksheet.Cells[row, 5], comparison.ResultAStatusCode);
            
            worksheet.Cells[row, 6].Value = comparison.ResultBStatusCode;
            FormatStatusCodeCell(worksheet.Cells[row, 6], comparison.ResultBStatusCode);
            
            // Timing information
            worksheet.Cells[row, 7].Value = comparison.TimeA;
            worksheet.Cells[row, 8].Value = comparison.TimeB;
            
            worksheet.Cells[row, 9].Value = comparison.TimeDifference;
            if (comparison.TimeDifference < 0)
            {
                worksheet.Cells[row, 9].Style.Font.Color.SetColor(Color.Green);
            }
            else if (comparison.TimeDifference > 0)
            {
                worksheet.Cells[row, 9].Style.Font.Color.SetColor(Color.Red);
            }

            // Request/Response bodies
            worksheet.Cells[row, 10].Value = comparison.requestData;
            worksheet.Cells[row, 10].Style.WrapText = true;

            worksheet.Cells[row, 11].Value = comparison.ResultASummary;
            worksheet.Cells[row, 11].Style.WrapText = true;

            worksheet.Cells[row, 12].Value = comparison.ResultBSummary;
            worksheet.Cells[row, 12].Style.WrapText = true;
            
            // Details (differences)
            if (comparison.Differences.Count > 0)
            {
                worksheet.Cells[row, 13].Value = string.Join("\n", comparison.Differences);
                worksheet.Cells[row, 13].Style.WrapText = true;
            }
            
            // Add border to all cells in the row
            for (int i = 1; i <= headers.Length; i++)
            {
                worksheet.Cells[row, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            
            row++;
        }
        
        // Response data sheet with full data
        CreateResponseDataSheet(package, results);
        
        // Auto-fit columns (except details which can be very long)
        for (int i = 1; i <= headers.Length - 1; i++)
        {
            worksheet.Column(i).AutoFit();
        }
        
        // Set a reasonable width for the details column
        worksheet.Column(headers.Length).Width = 50;
    }
    
    /// <summary>
    /// Creates a sheet containing the full response data
    /// </summary>
    private static void CreateResponseDataSheet(
        ExcelPackage package,
        Dictionary<string, ComparisonResult> results
        )
    {
        var worksheet = package.Workbook.Worksheets.Add("Response Data");
        
        // Title
        worksheet.Cells[1, 1].Value = "API Response Data";
        worksheet.Cells[1, 1].Style.Font.Size = 16;
        worksheet.Cells[1, 1].Style.Font.Bold = true;
        
        // Headers
        var headers = new[] 
        { 
            "Test Name", 
            "API Path/Endpoint", 
            "Method",
            "Status A", 
            "Status B", 
            "Response A", 
            "Response B" 
        };
        
        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cells[3, i + 1].Value = headers[i];
            worksheet.Cells[3, i + 1].Style.Font.Bold = true;
            worksheet.Cells[3, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[3, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[3, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }
        
        // Data rows
        int row = 4;
        foreach (var (testName, comparison) in results)
        {

            
            worksheet.Cells[row, 1].Value = testName;
            worksheet.Cells[row, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            
            worksheet.Cells[row, 2].Value = comparison.path;;
            worksheet.Cells[row, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            
            worksheet.Cells[row, 3].Value = comparison.method;
            worksheet.Cells[row, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            
            worksheet.Cells[row, 4].Value = comparison.ResultAStatusCode;
            FormatStatusCodeCell(worksheet.Cells[row, 4], comparison.ResultAStatusCode);
            worksheet.Cells[row, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            
            worksheet.Cells[row, 5].Value = comparison.ResultBStatusCode;
            FormatStatusCodeCell(worksheet.Cells[row, 5], comparison.ResultBStatusCode);
            worksheet.Cells[row, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            
            worksheet.Cells[row, 6].Value = comparison.ResultASummary;
            worksheet.Cells[row, 6].Style.WrapText = true;
            worksheet.Cells[row, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            
            worksheet.Cells[row, 7].Value = comparison.ResultBSummary;
            worksheet.Cells[row, 7].Style.WrapText = true;
            worksheet.Cells[row, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            
            row++;
        }
        
        // Set column widths
        worksheet.Column(1).Width = 25; // Test Name
        worksheet.Column(2).Width = 30; // API Path
        worksheet.Column(3).Width = 10; // Method
        worksheet.Column(4).Width = 10; // Status A
        worksheet.Column(5).Width = 10; // Status B
        worksheet.Column(6).Width = 60; // Response A
        worksheet.Column(7).Width = 60; // Response B
        
        // Set row heights to accommodate wrapped text
        for (int i = 4; i < row; i++)
        {
            worksheet.Row(i).Height = 100;
        }
    }
    
    
    /// <summary>
    /// Formats a cell containing a status code with appropriate colors
    /// </summary>
    private static void FormatStatusCodeCell(ExcelRange cell, int statusCode)
    {
        if (statusCode >= 200 && statusCode < 300)
        {
            // Success (2xx) - Green
            cell.Style.Font.Color.SetColor(Color.Green);
        }
        else if (statusCode >= 300 && statusCode < 400)
        {
            // Redirection (3xx) - Blue
            cell.Style.Font.Color.SetColor(Color.Blue);
        }
        else if (statusCode >= 400 && statusCode < 500)
        {
            // Client Error (4xx) - Orange
            cell.Style.Font.Color.SetColor(Color.Orange);
        }
        else if (statusCode >= 500)
        {
            // Server Error (5xx) - Red
            cell.Style.Font.Color.SetColor(Color.Red);
        }
    }


}
