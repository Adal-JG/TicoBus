using TicoBus.MAUI.DTOs;
using TicoBus.MAUI.Interfaces;
using TicoBus.Model;

namespace TicoBus.MAUI.Services
{
    public class MisViajesService : ApiClientBase, IMisViajesService
    {
        public async Task<ApiResponse<List<Reserva>>?> ListarPorPasajeroAsync(int pasajeroId)
        {
            return await GetApiResponseAsync<List<Reserva>>($"MisViajes/{pasajeroId}");
        }
    }
}