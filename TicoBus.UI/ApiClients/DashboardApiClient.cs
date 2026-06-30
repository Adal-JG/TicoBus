using System.Text.Json;
using TicoBus.Model;

namespace TicoBus.UI.ApiClients
{
    public class DashboardApiClient : ApiClientBase
    {
        public DashboardApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<JsonElement?> ObtenerAdmin()
        {
            var response = await GetAsync<JsonElement>("Dashboard/admin");
            return response?.Datos;
        }

        public async Task<JsonElement?> ObtenerChofer(int usuarioId)
        {
            var response = await GetAsync<JsonElement>($"Dashboard/chofer/{usuarioId}");
            return response?.Datos;
        }
    }
}