using TicoBus.BL.Interfaces;
using TicoBus.DA.Repositories;
using TicoBus.Model;

namespace TicoBus.BL.Services
{
    public class RutaService : IRutaService
    {
        private readonly RutaRepository _rutaRepository;

        public RutaService(RutaRepository rutaRepository)
        {
            _rutaRepository = rutaRepository;
        }

        public List<Ruta> Listar(string? filtro)
        {
            return _rutaRepository.Listar(filtro);
        }

        public Ruta? ObtenerPorId(int id)
        {
            return _rutaRepository.ObtenerPorId(id);
        }

        public bool Agregar(Ruta ruta, out string mensaje)
        {
            if (ruta.PrecioBase <= 0)
            {
                mensaje = "El precio base debe ser mayor a cero.";
                return false;
            }

            if (ruta.DuracionEstimada <= TimeSpan.Zero)
            {
                mensaje = "La duración estimada debe ser mayor a cero.";
                return false;
            }

            _rutaRepository.Agregar(ruta);

            mensaje = "Ruta agregada correctamente.";
            return true;
        }

        public bool Actualizar(Ruta ruta, out string mensaje)
        {
            var rutaActual = _rutaRepository.ObtenerPorId(ruta.Id);

            if (rutaActual == null)
            {
                mensaje = "Ruta no encontrada.";
                return false;
            }

            if (ruta.PrecioBase <= 0)
            {
                mensaje = "El precio base debe ser mayor a cero.";
                return false;
            }

            if (ruta.DuracionEstimada <= TimeSpan.Zero)
            {
                mensaje = "La duración estimada debe ser mayor a cero.";
                return false;
            }

            rutaActual.Nombre = ruta.Nombre;
            rutaActual.Origen = ruta.Origen;
            rutaActual.Destino = ruta.Destino;
            rutaActual.DuracionEstimada = ruta.DuracionEstimada;
            rutaActual.PrecioBase = ruta.PrecioBase;

            _rutaRepository.Actualizar(rutaActual);

            mensaje = "Ruta actualizada correctamente.";
            return true;
        }

        public bool Eliminar(int id, out string mensaje)
        {
            var ruta = _rutaRepository.ObtenerPorId(id);

            if (ruta == null)
            {
                mensaje = "Ruta no encontrada.";
                return false;
            }

            if (_rutaRepository.TieneViajes(id))
            {
                mensaje = "No se puede eliminar la ruta porque tiene viajes registrados.";
                return false;
            }

            _rutaRepository.Eliminar(ruta);

            mensaje = "Ruta eliminada correctamente.";
            return true;
        }
    }
}