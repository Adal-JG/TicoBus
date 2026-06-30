using System.Net.Http.Json;

namespace TicoBus.UI.ApiClients
{
    public class ApiClientBase
    {
        protected readonly HttpClient _httpClient;

        public ApiClientBase(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            var baseUrl = configuration["ApiSettings:BaseUrl"];
            var apiKey = configuration["ApiSettings:ApiKey"];

            _httpClient.BaseAddress = new Uri(baseUrl!);

            if (!_httpClient.DefaultRequestHeaders.Contains("X-API-KEY"))
            {
                _httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
            }
        }

        protected async Task<ApiResponse<T>?> GetAsync<T>(string url)
        {
            return await _httpClient.GetFromJsonAsync<ApiResponse<T>>(url);
        }

        protected async Task<ApiResponse<T>?> PostAsync<T>(string url, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data);
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
        }

        protected async Task<ApiResponse<T>?> PutAsync<T>(string url, object data)
        {
            var response = await _httpClient.PutAsJsonAsync(url, data);
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
        }
    }
}