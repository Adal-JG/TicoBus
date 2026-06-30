using TicoBus.Model;

namespace TicoBus.UI.ApiClients
{
    public class RutasApiClient : ApiClientBase
    {
        public RutasApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<List<Ruta>> Listar(string? filtro)
        {
            var url = string.IsNullOrWhiteSpace(filtro)
                ? "Rutas"
                : $"Rutas?filtro={Uri.EscapeDataString(filtro)}";

            var response = await GetAsync<List<Ruta>>(url);
            return response?.Datos ?? new List<Ruta>();
        }

        public async Task<Ruta?> ObtenerPorId(int id)
        {
            var response = await GetAsync<Ruta>($"Rutas/{id}");
            return response?.Datos;
        }

        public async Task<ApiResponse<Ruta>?> Agregar(Ruta ruta)
        {
            return await PostAsync<Ruta>("Rutas", ruta);
        }

        public async Task<ApiResponse<Ruta>?> Actualizar(int id, Ruta ruta)
        {
            return await PutAsync<Ruta>($"Rutas/{id}", ruta);
        }
    }
}