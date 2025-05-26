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
    /// <returns>True if export was successful, false otherwise</returns>
    public static bool ExportToExcel<T>(
        Dictionary<string, (T? ResultA, T? ResultB, ComparisonResult Comparison)> results,
        string filePath)
    {
        try
        {
            // Set license context for EPPlus (commercial use requires license)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using var package = new ExcelPackage();
            
            // Create summary worksheet
            CreateSummarySheet(package, results);
            
            // Create detailed results worksheet
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
    /// Creates a summary sheet with overview of test results
    /// </summary>
    private static void CreateSummarySheet<T>(
        ExcelPackage package, 
        Dictionary<string, (T? ResultA, T? ResultB, ComparisonResult Comparison)> results)
    {
        var worksheet = package.Workbook.Worksheets.Add("Summary");
        
        // Title and timestamp
        worksheet.Cells[1, 1].Value = "API Test Results Summary";
        worksheet.Cells[1, 1].Style.Font.Size = 16;
        worksheet.Cells[1, 1].Style.Font.Bold = true;
        
        worksheet.Cells[2, 1].Value = "Generated:";
        worksheet.Cells[2, 2].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        // Statistics
        worksheet.Cells[4, 1].Value = "Statistics";
        worksheet.Cells[4, 1].Style.Font.Bold = true;
        
        worksheet.Cells[5, 1].Value = "Total Tests:";
        worksheet.Cells[5, 2].Value = results.Count;
        
        var successCount = results.Count(r => r.Value.Comparison.ResultsEqual);
        var failureCount = results.Count - successCount;
        
        worksheet.Cells[6, 1].Value = "Successful Tests:";
        worksheet.Cells[6, 2].Value = successCount;
        
        worksheet.Cells[7, 1].Value = "Failed Tests:";
        worksheet.Cells[7, 2].Value = failureCount;
        
        worksheet.Cells[8, 1].Value = "Success Rate:";
        worksheet.Cells[8, 2].Value = results.Count > 0 
            ? (double)successCount / results.Count 
            : 0;
        worksheet.Cells[8, 2].Style.Numberformat.Format = "0.00%";
        
        // Average timings
        if (results.Any())
        {
            var avgTimeA = results.Average(r => r.Value.Comparison.TimeA);
            var avgTimeB = results.Average(r => r.Value.Comparison.TimeB);
            var avgTimeDiff = results.Average(r => r.Value.Comparison.TimeDifference);
            
            worksheet.Cells[10, 1].Value = "Average Response Times";
            worksheet.Cells[10, 1].Style.Font.Bold = true;
            
            worksheet.Cells[11, 1].Value = "Service A:";
            worksheet.Cells[11, 2].Value = avgTimeA;
            worksheet.Cells[11, 2].Style.Numberformat.Format = "0.00 \"ms\"";
            
            worksheet.Cells[12, 1].Value = "Service B:";
            worksheet.Cells[12, 2].Value = avgTimeB;
            worksheet.Cells[12, 2].Style.Numberformat.Format = "0.00 \"ms\"";
            
            worksheet.Cells[13, 1].Value = "Difference (A-B):";
            worksheet.Cells[13, 2].Value = avgTimeDiff;
            worksheet.Cells[13, 2].Style.Numberformat.Format = "0.00 \"ms\"";
            
            // Set color based on difference (green if A is faster, red if B is faster)
            if (avgTimeDiff < 0)
            {
                worksheet.Cells[13, 2].Style.Font.Color.SetColor(Color.Green);
            }
            else if (avgTimeDiff > 0)
            {
                worksheet.Cells[13, 2].Style.Font.Color.SetColor(Color.Red);
            }
        }
        
        // Test result overview table
        worksheet.Cells[15, 1].Value = "Test Results Overview";
        worksheet.Cells[15, 1].Style.Font.Bold = true;
        
        // Headers
        var headers = new[] { "Test Name", "Status", "Status A", "Status B", "Time A (ms)", "Time B (ms)", "Diff (ms)" };
        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cells[16, i + 1].Value = headers[i];
            worksheet.Cells[16, i + 1].Style.Font.Bold = true;
            worksheet.Cells[16, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[16, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[16, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }
        
        // Data rows
        int row = 17;
        foreach (var (testName, (_, _, comparison)) in results)
        {
            worksheet.Cells[row, 1].Value = testName;
            
            worksheet.Cells[row, 2].Value = comparison.ResultsEqual ? "Success" : "Failure";
            worksheet.Cells[row, 2].Style.Font.Color.SetColor(comparison.ResultsEqual ? Color.Green : Color.Red);
            
            worksheet.Cells[row, 3].Value = comparison.ResultAStatusCode;
            FormatStatusCodeCell(worksheet.Cells[row, 3], comparison.ResultAStatusCode);
            
            worksheet.Cells[row, 4].Value = comparison.ResultBStatusCode;
            FormatStatusCodeCell(worksheet.Cells[row, 4], comparison.ResultBStatusCode);
            
            worksheet.Cells[row, 5].Value = comparison.TimeA;
            worksheet.Cells[row, 6].Value = comparison.TimeB;
            
            worksheet.Cells[row, 7].Value = comparison.TimeDifference;
            if (comparison.TimeDifference < 0)
            {
                worksheet.Cells[row, 7].Style.Font.Color.SetColor(Color.Green);
            }
            else if (comparison.TimeDifference > 0)
            {
                worksheet.Cells[row, 7].Style.Font.Color.SetColor(Color.Red);
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
    private static void CreateDetailedResultsSheet<T>(
        ExcelPackage package, 
        Dictionary<string, (T? ResultA, T? ResultB, ComparisonResult Comparison)> results)
    {
        var worksheet = package.Workbook.Worksheets.Add("Detailed Results");
        
        // Title
        worksheet.Cells[1, 1].Value = "API Test Detailed Results";
        worksheet.Cells[1, 1].Style.Font.Size = 16;
        worksheet.Cells[1, 1].Style.Font.Bold = true;
        
        // Headers
        var headers = new[] 
        { 
            "Test Name", 
            "API Path",
            "Equal",
            "Status A", 
            "Status B", 
            "Time A (ms)", 
            "Time B (ms)", 
            "Diff (ms)",
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
        int row = 4;
        foreach (var (testName, (resultA, resultB, comparison)) in results)
        {
            // Extract API path from test name if possible
            string apiPath = GetApiPathFromTestName(testName);
            
            worksheet.Cells[row, 1].Value = testName;
            worksheet.Cells[row, 2].Value = apiPath;
            
            // Equal status with color coding
            worksheet.Cells[row, 3].Value = comparison.ResultsEqual;
            worksheet.Cells[row, 3].Style.Font.Color.SetColor(comparison.ResultsEqual ? Color.Green : Color.Red);
            
            // Status codes
            worksheet.Cells[row, 4].Value = comparison.ResultAStatusCode;
            FormatStatusCodeCell(worksheet.Cells[row, 4], comparison.ResultAStatusCode);
            
            worksheet.Cells[row, 5].Value = comparison.ResultBStatusCode;
            FormatStatusCodeCell(worksheet.Cells[row, 5], comparison.ResultBStatusCode);
            
            // Timing information
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
            
            // Details (differences)
            if (comparison.Differences.Count > 0)
            {
                worksheet.Cells[row, 9].Value = string.Join("\n", comparison.Differences);
                worksheet.Cells[row, 9].Style.WrapText = true;
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
    private static void CreateResponseDataSheet<T>(
        ExcelPackage package,
        Dictionary<string, (T? ResultA, T? ResultB, ComparisonResult Comparison)> results)
    {
        var worksheet = package.Workbook.Worksheets.Add("Response Data");
        
        // Title
        worksheet.Cells[1, 1].Value = "API Response Data";
        worksheet.Cells[1, 1].Style.Font.Size = 16;
        worksheet.Cells[1, 1].Style.Font.Bold = true;
        
        // Headers
        var headers = new[] { "Test Name", "Response A", "Response B" };
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
        foreach (var (testName, (_, _, comparison)) in results)
        {
            worksheet.Cells[row, 1].Value = testName;
            worksheet.Cells[row, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            
            worksheet.Cells[row, 2].Value = comparison.ResultASummary;
            worksheet.Cells[row, 2].Style.WrapText = true;
            worksheet.Cells[row, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            
            worksheet.Cells[row, 3].Value = comparison.ResultBSummary;
            worksheet.Cells[row, 3].Style.WrapText = true;
            worksheet.Cells[row, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            
            row++;
        }
        
        // Set column widths
        worksheet.Column(1).Width = 25;
        worksheet.Column(2).Width = 60;
        worksheet.Column(3).Width = 60;
        
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
    
    /// <summary>
    /// Extracts an API path from a test name
    /// </summary>
    private static string GetApiPathFromTestName(string testName)
    {
        // Try to find patterns like "/api/endpoint" in the test name
        if (testName.Contains("/"))
        {
            var parts = testName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (part.StartsWith("/"))
                {
                    return part;
                }
            }
        }
        
        // If no path found, return a placeholder
        return "N/A";
    }
}
