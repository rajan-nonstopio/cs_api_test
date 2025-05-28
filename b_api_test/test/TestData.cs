using System.Net.Http;
using System.Collections.Generic;

namespace b_api_test.test;

/// <summary>
/// Custom test data class - Users can add their test data here
/// </summary>
public static class TestData
{
    /// <summary>
    /// List of test API details that users can modify
    /// </summary>
    /// 
    /// Update here
    public static readonly List<TestApiDetails> CustomTestData = [
           new TestApiDetails  {
            TestName = "Sample api test",
            Method = HttpMethod.Get,
            Endpoint = "/api/test/",
        },
    ];
}