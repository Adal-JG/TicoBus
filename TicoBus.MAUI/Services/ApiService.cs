using System.Net.Http.Json;
using TicoBus.MAUI.Interfaces;

namespace TicoBus.MAUI.Services
{
    public class ApiService : ApiClientBase, IApiService
    {
        public async Task<T?> GetAsync<T>(string url)
        {
            return await _httpClient.GetFromJsonAsync<T>(url);
        }

        public async Task<T?> PostAsync<T>(string url, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data);
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}