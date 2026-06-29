using TicoBus.Model;

namespace TicoBus.BL.Interfaces
{
    public interface IUnidadService
    {
        List<Unidad> Listar(string? filtro);
        Unidad? ObtenerPorId(int id);
        bool Agregar(Unidad unidad, out string mensaje);
        bool Actualizar(Unidad unidad, out string mensaje);
        bool Eliminar(int id, out string mensaje);
    }
}