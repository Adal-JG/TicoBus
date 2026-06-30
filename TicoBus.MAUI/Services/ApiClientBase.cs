using System.Net.Http.Json;
using TicoBus.MAUI.DTOs;

namespace TicoBus.MAUI.Services
{
    public class ApiClientBase
    {
        protected readonly HttpClient _httpClient;

        public ApiClientBase()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://ticobus-api-c34106-a3gcbgg8gvbsfwe3.eastus-01.azurewebsites.net/api/")
            };

            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", "TicoBus-SegundaEntrega-2026");
        }

        protected async Task<ApiResponse<T>?> GetApiResponseAsync<T>(string url)
        {
            return await _httpClient.GetFromJsonAsync<ApiResponse<T>>(url);
        }

        protected async Task<ApiResponse<T>?> PostApiResponseAsync<T>(string url, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data);
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
        }
    }
}