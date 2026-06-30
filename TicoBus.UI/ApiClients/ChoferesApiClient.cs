using TicoBus.Model;

namespace TicoBus.UI.ApiClients
{
    public class ChoferesApiClient : ApiClientBase
    {
        public ChoferesApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<List<Chofer>> Listar(string? filtro)
        {
            var url = string.IsNullOrWhiteSpace(filtro)
                ? "Choferes"
                : $"Choferes?filtro={Uri.EscapeDataString(filtro)}";

            var response = await GetAsync<List<Chofer>>(url);
            return response?.Datos ?? new List<Chofer>();
        }

        public async Task<Chofer?> ObtenerPorId(int id)
        {
            var response = await GetAsync<Chofer>($"Choferes/{id}");
            return response?.Datos;
        }

        public async Task<ApiResponse<Chofer>?> Agregar(Chofer chofer)
        {
            return await PostAsync<Chofer>("Choferes", chofer);
        }

        public async Task<ApiResponse<Chofer>?> Actualizar(int id, Chofer chofer)
        {
            return await PutAsync<Chofer>($"Choferes/{id}", chofer);
        }
    }
}