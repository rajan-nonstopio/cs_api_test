using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace b_api_test;

/// <summary>
/// Service for making REST API calls with configurable base URL and headers
/// </summary>
public class ApiService
{
    private readonly HttpClient _httpClient;
    private string _baseUrl;
    
    /// <summary>
    /// Initializes a new instance of the ApiService class
    /// </summary>
    public ApiService()
    {
        _httpClient = new HttpClient();
        _baseUrl = string.Empty;
    }
    
    /// <summary>
    /// Sets the base URL for all API requests
    /// </summary>
    /// <param name="baseUrl">The base URL for the API</param>
    /// <returns>The current ApiService instance for method chaining</returns>
    public ApiService SetBaseUrl(string baseUrl)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        return this;
    }
    
    /// <summary>
    /// Sets the authorization token for all API requests
    /// </summary>
    /// <param name="token">The authorization token</param>
    /// <param name="scheme">The authorization scheme (default: Bearer)</param>
    /// <returns>The current ApiService instance for method chaining</returns>
    public ApiService SetToken(string token, string scheme = "Bearer")
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
        return this;
    }
    
    /// <summary>
    /// Adds a header to all API requests
    /// </summary>
    /// <param name="name">The header name</param>
    /// <param name="value">The header value</param>
    /// <returns>The current ApiService instance for method chaining</returns>
    public ApiService AddHeader(string name, string value)
    {
        _httpClient.DefaultRequestHeaders.Remove(name);
        _httpClient.DefaultRequestHeaders.Add(name, value);
        return this;
    }

    /// <summary>
    /// Performs a GET request to the specified endpoint
    /// </summary>
    /// <param name="endpoint">The API endpoint (will be appended to the base URL)</param>
    /// <returns>The deserialized response</returns>
    public async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint.TrimStart('/')}");
        return response;
    }

    /// <summary>
    /// Performs a POST request to the specified endpoint
    /// </summary>
    /// <param>The API endpoint (will be appended to the base URL)</param>
    /// <param name="endpoint"></param>
    /// <param name="data">The data to send in the request body</param>
    /// <returns>The deserialized response</returns>
    public async Task<HttpResponseMessage> PostAsync(string endpoint, object data)
    {
        var content = CreateJsonContent(data);
        var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint.TrimStart('/')}", content);
        return response;
    }


    /// <summary>
    /// Performs a PUT request to the specified endpoint
    /// </summary>
    /// <param name="endpoint">The API endpoint (will be appended to the base URL)</param>
    /// <param name="data">The data to send in the request body</param>
    /// <returns>The deserialized response</returns>
    public async Task<HttpResponseMessage> PutAsync(string endpoint, object data)
    {
        var content = CreateJsonContent(data);
        var response = await _httpClient.PutAsync($"{_baseUrl}/{endpoint.TrimStart('/')}", content);
        return response;
    }
    

    
    /// <summary>
    /// Performs a PATCH request to the specified endpoint
    /// </summary>
    /// <param name="endpoint">The API endpoint (will be appended to the base URL)</param>
    /// <param name="data">The data to send in the request body</param>
    /// <returns>The deserialized response</returns>
    public async Task<HttpResponseMessage> PatchAsync(string endpoint, object data)
    {
        var content = CreateJsonContent(data);
        var response = await _httpClient.PatchAsync($"{_baseUrl}/{endpoint.TrimStart('/')}", content);
        return response;
    }
    
    
    /// <summary>
    /// Performs a DELETE request to the specified endpoint
    /// </summary>
    /// <param name="endpoint">The API endpoint (will be appended to the base URL)</param>
    /// <returns>The deserialized response</returns>
    public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/{endpoint.TrimStart('/')}");
        return response;
    }
    

    /// <summary>
    /// Creates JSON content from an object
    /// </summary>
    /// <param name="data">The data to serialize</param>
    /// <returns>StringContent with JSON media type</returns>
    private static StringContent CreateJsonContent(object data)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
    
}
