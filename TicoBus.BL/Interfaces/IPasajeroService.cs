using TicoBus.Model;

namespace TicoBus.BL.Interfaces
{
    public interface IPasajeroService
    {
        List<Pasajero> Listar(string? filtro);
        Pasajero? ObtenerPorId(int id);
        bool Agregar(Pasajero pasajero, out string mensaje);
        bool Actualizar(Pasajero pasajero, out string mensaje);
        bool Eliminar(int id, out string mensaje);
        Pasajero? ObtenerPorUsuarioId(int usuarioId);
    }
}
