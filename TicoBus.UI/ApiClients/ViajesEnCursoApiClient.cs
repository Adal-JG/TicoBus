using TicoBus.Model;

namespace TicoBus.UI.ApiClients
{
    public class ViajesEnCursoApiClient : ApiClientBase
    {
        public ViajesEnCursoApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<List<Viaje>> Listar()
        {
            var response = await GetAsync<List<Viaje>>("ViajesEnCurso");
            return response?.Datos ?? new List<Viaje>();
        }

        public async Task<Viaje?> ObtenerPorId(int id)
        {
            var response = await GetAsync<Viaje>($"ViajesEnCurso/{id}");
            return response?.Datos;
        }

        public async Task<List<Reserva>> ListarReservas(int viajeId)
        {
            var response = await GetAsync<List<Reserva>>($"ViajesEnCurso/{viajeId}/reservas");
            return response?.Datos ?? new List<Reserva>();
        }

        public async Task<ApiResponse<object>?> Reservar(int viajeId, int pasajeroId, int numeroAsiento)
        {
            var request = new
            {
                ViajeId = viajeId,
                PasajeroId = pasajeroId,
                NumeroAsiento = numeroAsiento
            };

            return await PostAsync<object>("ViajesEnCurso/reservar", request);
        }

        public async Task<ApiResponse<object>?> CancelarReserva(int reservaId)
        {
            var request = new
            {
                ReservaId = reservaId
            };

            return await PostAsync<object>("ViajesEnCurso/cancelar-reserva", request);
        }

        public async Task<ApiResponse<object>?> Finalizar(int id)
        {
            return await PostAsync<object>($"ViajesEnCurso/{id}/finalizar", new { });
        }
    }
}