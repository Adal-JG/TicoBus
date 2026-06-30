using TicoBus.Model;

namespace TicoBus.UI.ApiClients
{
    public class UnidadesApiClient : ApiClientBase
    {
        public UnidadesApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<List<Unidad>> Listar(string? filtro)
        {
            var url = string.IsNullOrWhiteSpace(filtro)
                ? "Unidades"
                : $"Unidades?filtro={Uri.EscapeDataString(filtro)}";

            var response = await GetAsync<List<Unidad>>(url);
            return response?.Datos ?? new List<Unidad>();
        }

        public async Task<Unidad?> ObtenerPorId(int id)
        {
            var response = await GetAsync<Unidad>($"Unidades/{id}");
            return response?.Datos;
        }

        public async Task<ApiResponse<Unidad>?> Agregar(Unidad unidad)
        {
            return await PostAsync<Unidad>("Unidades", unidad);
        }

        public async Task<ApiResponse<Unidad>?> Actualizar(int id, Unidad unidad)
        {
            return await PutAsync<Unidad>($"Unidades/{id}", unidad);
        }
    }
}