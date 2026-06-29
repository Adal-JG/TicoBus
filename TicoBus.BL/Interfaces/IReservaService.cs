using TicoBus.Model;

namespace TicoBus.BL.Interfaces
{
    public interface IReservaService
    {
        List<Reserva> ListarPorViaje(int viajeId);
        bool Reservar(int viajeId, int pasajeroId, int numeroAsiento, out string mensaje);
        bool CancelarReserva(int reservaId, out string mensaje);
        int CantidadReservasActivas(int viajeId);
        decimal TotalRecaudado(int viajeId);
    }
}