# 🚀 API Testing Tool & OpenAPI Test Case Generator

A lightweight CLI tool for comparing API responses between environments and automatically generating test cases from OpenAPI specifications.

## 📋 Table of Contents

- [Prerequisites](#prerequisites)
- [Configuration Setup](#configuration-setup)
- [OpenAPI Test Case Generator](#-openapi-test-case-generator-module)
- [API Testing Tool](#api-testing-tool)
- [Prerequisites](#-prerequisites)
- [Getting Started](#-getting-started)
- [Usage](#-usage)
- [Adding or Modifying Test Cases](#adding-or-modifying-test-cases)
- [How It Works](#how-it-works)
- [Running the Tool](#running-the-tool)
- [Output](#output)
- [Project Structure](#project-structure2)


## ✅ Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download)
- IDE (e.g., Visual Studio, JetBrains Rider)
- Access to both API environments (e.g., Staging & Production)

---

## ⚙️ Configuration Setup

### Create `appsettings.json` in the project root:

```json
{
  "ApiSettings": {
    "BaseUrl1": "https://api-env1.example.com",
    "BaseUrl2": "https://api-env2.example.com",
    "Token1": "your_token_for_env1",
    "Token2": "your_token_for_env2"
  }
}
```

- `BaseUrl1`, `BaseUrl2`: Base URLs for the APIs you want to compare.
- `Token1`, `Token2`: Optional authentication tokens.

> Use `appsettings.example.json` as a reference template.

---


## 🧰 OpenAPI Test Case Generator Module

This module parses an OpenAPI/Swagger JSON file and automatically generates `TestApiDetails` objects, which can then be used by the API Testing Tool.

### ✅ Prerequisites (Generator)

- .NET 8.0 SDK
- IDE (e.g., Visual Studio, JetBrains Rider)

### 📂 Setup (Generator)

1. Create a `test` directory in your project root:
   ```bash
   mkdir test
   ```
2. Place your OpenAPI/Swagger JSON specification file into the `test` folder, named as `sample.json`.

### ▶️ Usage (Generator)

To generate your test cases, you will typically use the `OpenApiTestGenerator` class.

**Basic Usage**
```csharp
// Generate test API details from 'test/sample.json'
var testApiDetailsList = OpenApiTestGenerator.GenerateTestApiDetails();

// Export the generated list to a C# file named 'GeneratedTestApiDetails.cs'
// This file will contain the TestApiDetails class and a static method to get the list.
OpenApiTestGenerator.ExportToFile(testApiDetailsList, "GeneratedTestApiDetails.cs");
```

**Custom File Paths**
```csharp
// Using a custom input OpenAPI specification file
var testApiDetailsList = OpenApiTestGenerator.GenerateTestApiDetails("path/to/your/custom_spec.json");

// Exporting the generated test cases to a custom output location and filename
OpenApiTestGenerator.ExportToFile(testApiDetailsList, "path/to/output/MyGeneratedTests.cs");
```

### 📤 Generated Output

The generator creates a C# file (e.g., `GeneratedTestApiDetails.cs`) containing:

A `TestApiDetails` class with properties:

```csharp
public class TestApiDetails
{
    public string Endpoint { get; set; }
    public HttpMethod Method { get; set; }
    public object? Data { get; set; }
    public string TestName { get; set; }
}
```

A static `GeneratedTestApiDetails` class with a method to get all generated test cases as a `List<TestApiDetails>`. For example, if your sample.json described a `/api/users` GET endpoint and a `/api/products` POST endpoint, the `GetTestApiDetailsList()` method might return something like this:

```csharp
public static class GeneratedTestApiDetails
{
    public static List<TestApiDetails> GetTestApiDetailsList()
    {
        return new List<TestApiDetails>
        {
            new TestApiDetails
            {
                Endpoint = "/api/users",
                Method = HttpMethod.Get,
                Data = null,
                TestName = "GET_users_GetAllUsers"
            },
            new TestApiDetails
            {
                Endpoint = "/api/products",
                Method = HttpMethod.Post,
                Data = new
                {
                    productId = "PROD001",
                    name = "Example Product",
                    price = 99.99
                },
                TestName = "POST_products_CreateProduct"
            },
            // ... more test cases generated from your OpenAPI spec
        };
    }
}
```
The generator attempts to create realistic sample data based on the schema types and names.

### 🗂️ File Structure (Generator)

```
your-project-root/
├── test/
│   └── sample.json              # Your OpenAPI specification file
├── Program.cs                   # Main execution file (where you might call the generator)
├── OpenApiTestGenerator.cs      # Core logic for parsing OpenAPI and generating test cases
└── GeneratedTestApiDetails.cs   # The C# file containing the auto-generated test cases
```



## ✍️ Adding or Modifying Test Cases

Test cases are defined in `TestData.cs` as a list of `TestApiDetails`.
Below list can be also copied from above steps

Each test includes:
- `TestName`: A label for identification.
- `Method`: HTTP method (`GET`, `POST`, etc.).
- `Endpoint`: API path (relative to base URL).
- `Data`: (Optional) Request body for applicable methods.

### Example:
```csharp
    public static readonly List<TestApiDetails> CustomTestData = [
           new TestApiDetails  {
            TestName = "Sample api test",
            Method = HttpMethod.Get,
            Endpoint = "/api/test/",
        },
    ];
```

Edit or extend the `CustomTestData` list in `TestData.cs`.

---

## 🔍 How It Works

- **`Program.cs`**:
  - Loads configuration from `appsettings.json`.
  - Initializes the API tester via `Config.CreateDefaultTester()`.
  - Loads the test suite from `TestData.CustomTestData`.
  - Executes all test cases against both environments.
  - Outputs a console summary and generates an Excel report.

- **`Config.cs`**: Handles configuration loading and tester setup.

- **`TestApiDetails.cs`**: Defines the test case model.

---

## ▶️ Running the Tool

1. Ensure `appsettings.json` is correctly configured.
2. Add or update your test cases in `TestData.cs`.
3. Build and run the project:

```bash
dotnet run --project b_api_test/b_api_test.csproj
```

4. Watch the console for test results.
5. View the Excel report (e.g., `api_results_20250429_134502.xlsx`) in the root directory.

---

## 📊 Output

### Console:
- Summary table of test cases with:
  - Test name
  - Status codes
  - Response times
  - Equality result

### Excel:
- Detailed comparison including headers, bodies, and differences.

---

## 📁 Project Structure

| File              | Purpose                                 |
|-------------------|------------------------------------------|
| `Program.cs`      | Entry point, runs tests and exports results |
| `Config.cs`       | Loads config and initializes tester      |
| `TestData.cs`     | Holds the list of test cases             |
| `ExcelExporter.cs`| Exports results to an Excel file         |

---

Feel free to contribute or fork this repo to fit your API testing needs.