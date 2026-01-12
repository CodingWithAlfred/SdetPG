using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Net.Http;

namespace RestAPIApp.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiClient> _logger;
        private readonly string _baseUrl;

        public ApiClient(string baseUrl, Dictionary<string, string> headers, ILogger<ApiClient> logger)
        {
            _baseUrl = baseUrl;
            _logger = logger;
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };

            foreach (var header in headers)
            {
                if (string.IsNullOrWhiteSpace(header.Key))
                    throw new ArgumentException("Header name cannot be null or whitespace.", nameof(headers));
            
                if (header.Key.Equals("Accept", StringComparison.OrdinalIgnoreCase))
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            _logger.LogInformation("GET Request to: {Endpoint}", endpoint);
            var response = await _httpClient.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("GET Response Status: {StatusCode}", response.StatusCode);
            response.EnsureSuccessStatusCode();  

            return JsonSerializer.Deserialize<T>(content);
        }

        public async Task<HttpResponseMessage> GetAsyncRaw(string endpoint)
        {
            _logger.LogInformation("GET Request to: {Endpoint}", endpoint);
            var response = await _httpClient.GetAsync(endpoint);
            _logger.LogInformation("GET Response Status: {StatusCode}", response.StatusCode);
            return response;
        }

        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            _logger.LogInformation("POST Request to: {Endpoint}", endpoint);
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("POST Response Status: {StatusCode}", response.StatusCode);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<T>(responseContent);
        }

        public async Task<HttpResponseMessage> PostAsyncRaw(string endpoint, object data)
        {
            _logger.LogInformation("POST Request to: {Endpoint}", endpoint);
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            _logger.LogInformation("POST Response Status: {StatusCode}", response.StatusCode);
            return response;
        }

        public async Task<T?> PutAsync<T>(string endpoint, object data)
        {
            _logger.LogInformation("PUT Request to: {Endpoint}", endpoint);
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("PUT Response Status: {StatusCode}", response.StatusCode);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<T>(responseContent);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            _logger.LogInformation("DELETE Request to: {Endpoint}", endpoint);
            var response = await _httpClient.DeleteAsync(endpoint);
            _logger.LogInformation("DELETE Response Status: {StatusCode}", response.StatusCode);
            return response;
        }
    }
}
