using TicoBus.Model;

namespace TicoBus.UI.ApiClients
{
    public class MisViajesApiClient : ApiClientBase
    {
        public MisViajesApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<List<Reserva>> ListarPorUsuario(int usuarioId)
        {
            var response = await GetAsync<List<Reserva>>($"MisViajes/{usuarioId}");
            return response?.Datos ?? new List<Reserva>();
        }
    }
}