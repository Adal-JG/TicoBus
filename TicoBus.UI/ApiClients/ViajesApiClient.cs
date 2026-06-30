using TicoBus.Model;

namespace TicoBus.UI.ApiClients
{
    public class ViajesApiClient : ApiClientBase
    {
        public ViajesApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<List<Viaje>> Listar(string? filtro)
        {
            var url = string.IsNullOrWhiteSpace(filtro)
                ? "Viajes"
                : $"Viajes?filtro={Uri.EscapeDataString(filtro)}";

            var response = await GetAsync<List<Viaje>>(url);
            return response?.Datos ?? new List<Viaje>();
        }

        public async Task<Viaje?> ObtenerPorId(int id)
        {
            var response = await GetAsync<Viaje>($"Viajes/{id}");
            return response?.Datos;
        }

        public async Task<ApiResponse<Viaje>?> Agregar(Viaje viaje)
        {
            return await PostAsync<Viaje>("Viajes", viaje);
        }

        public async Task<ApiResponse<Viaje>?> Actualizar(int id, Viaje viaje)
        {
            return await PutAsync<Viaje>($"Viajes/{id}", viaje);
        }

        public async Task<ApiResponse<object>?> Iniciar(int id)
        {
            return await PostAsync<object>($"Viajes/{id}/iniciar", new { });
        }

        public async Task<ApiResponse<object>?> Cancelar(int id, string motivo)
        {
            return await PostAsync<object>($"Viajes/{id}/cancelar", motivo);
        }
    }
}