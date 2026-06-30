using TicoBus.MAUI.DTOs;
using TicoBus.MAUI.Interfaces;
using TicoBus.Model;

namespace TicoBus.MAUI.Services
{
    public class PasajeroService : ApiClientBase, IPasajeroService
    {
        public async Task<ApiResponse<List<Pasajero>>?> ListarAsync()
        {
            return await GetApiResponseAsync<List<Pasajero>>("Pasajeros");
        }
    }
}