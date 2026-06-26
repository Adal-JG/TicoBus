using TicoBus.Model;

namespace TicoBus.BL.Interfaces
{
    public interface IChoferService
    {
        List<Chofer> Listar(string? filtro);
        Chofer? ObtenerPorId(int id);
        bool Agregar(Chofer chofer, out string mensaje);
        bool Actualizar(Chofer chofer, out string mensaje);
        bool TieneViajes(int choferId);
        bool Eliminar(int id, out string mensaje);
    }
}