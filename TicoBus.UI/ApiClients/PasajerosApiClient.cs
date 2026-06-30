using TicoBus.Model;

namespace TicoBus.UI.ApiClients
{
    public class PasajerosApiClient : ApiClientBase
    {
        public PasajerosApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<List<Pasajero>> Listar(string? filtro)
        {
            var url = string.IsNullOrWhiteSpace(filtro)
                ? "Pasajeros"
                : $"Pasajeros?filtro={Uri.EscapeDataString(filtro)}";

            var response = await GetAsync<List<Pasajero>>(url);

            return response?.Datos ?? new();
        }

        public async Task<Pasajero?> ObtenerPorId(int id)
        {
            var response = await GetAsync<Pasajero>($"Pasajeros/{id}");

            return response?.Datos;
        }

        public async Task<ApiResponse<Pasajero>?> Agregar(Pasajero pasajero)
        {
            return await PostAsync<Pasajero>("Pasajeros", pasajero);
        }

        public async Task<ApiResponse<Pasajero>?> Actualizar(int id, Pasajero pasajero)
        {
            return await PutAsync<Pasajero>($"Pasajeros/{id}", pasajero);
        }
    }
}