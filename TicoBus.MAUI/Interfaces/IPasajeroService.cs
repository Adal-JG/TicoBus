using TicoBus.MAUI.DTOs;
using TicoBus.Model;

namespace TicoBus.MAUI.Interfaces
{
    public interface IPasajeroService
    {
        Task<ApiResponse<List<Pasajero>>?> ListarAsync();
    }
}