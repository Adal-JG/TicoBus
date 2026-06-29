using TicoBus.Model;

namespace TicoBus.BL.Interfaces
{
    public interface IViajeService
    {
        List<Viaje> Listar(string? filtro);
        Viaje? ObtenerPorId(int id);
        bool Agregar(Viaje viaje, out string mensaje);
        bool Actualizar(Viaje viaje, out string mensaje);
        bool Iniciar(int id, out string mensaje);
        bool Cancelar(int id, string motivo, out string mensaje);
    }
}