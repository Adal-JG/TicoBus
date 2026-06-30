using TicoBus.MAUI.DTOs;
using TicoBus.Model;

namespace TicoBus.MAUI.Interfaces
{
    public interface IMisViajesService
    {
        Task<ApiResponse<List<Reserva>>?> ListarPorPasajeroAsync(int pasajeroId);
    }
}