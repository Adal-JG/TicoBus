using TicoBus.Model;

namespace TicoBus.BL.Interfaces
{
    public interface IRutaService
    {
        List<Ruta> Listar(string? filtro);
        Ruta? ObtenerPorId(int id);
        bool Agregar(Ruta ruta, out string mensaje);
        bool Actualizar(Ruta ruta, out string mensaje);
        bool Eliminar(int id, out string mensaje);
    }
}