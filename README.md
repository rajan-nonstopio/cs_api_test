# 🚀 API Testing Tool

A lightweight CLI tool to compare API responses between two environments (e.g., staging vs production) and export detailed results to Excel.

---

## 📋 Table of Contents

- [Prerequisites](#prerequisites)
- [Configuration Setup](#configuration-setup)
- [Adding or Modifying Test Cases](#adding-or-modifying-test-cases)
- [How It Works](#how-it-works)
- [Running the Tool](#running-the-tool)
- [Output](#output)
- [Project Structure](#project-structure)

---

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

## ✍️ Adding or Modifying Test Cases

Test cases are defined in `TestData.cs` as a list of `TestApiDetails`.

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