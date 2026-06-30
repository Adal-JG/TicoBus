using TicoBus.Model;

namespace TicoBus.UI.ApiClients
{
    public class ViajesCanceladosApiClient : ApiClientBase
    {
        public ViajesCanceladosApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<List<Viaje>> Listar()
        {
            var response = await GetAsync<List<Viaje>>("ViajesCancelados");
            return response?.Datos ?? new List<Viaje>();
        }

        public async Task<Viaje?> ObtenerPorId(int id)
        {
            var response = await GetAsync<Viaje>($"ViajesCancelados/{id}");
            return response?.Datos;
        }
    }
}