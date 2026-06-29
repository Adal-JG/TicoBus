using TicoBus.BL.Interfaces;
using TicoBus.DA.Repositories;
using TicoBus.Model;

namespace TicoBus.BL.Services
{
    public class UnidadService : IUnidadService
    {
        private readonly UnidadRepository _unidadRepository;

        public UnidadService(UnidadRepository unidadRepository)
        {
            _unidadRepository = unidadRepository;
        }

        public List<Unidad> Listar(string? filtro)
        {
            return _unidadRepository.Listar(filtro);
        }

        public Unidad? ObtenerPorId(int id)
        {
            return _unidadRepository.ObtenerPorId(id);
        }

        public bool Agregar(Unidad unidad, out string mensaje)
        {
            unidad.Placa = unidad.Placa.Trim().ToUpper();

            if (_unidadRepository.ExistePlaca(unidad.Placa))
            {
                mensaje = "Ya existe una unidad registrada con esa placa.";
                return false;
            }

            if (unidad.AnioFabricacion < 1980 || unidad.AnioFabricacion > DateTime.Now.Year + 1)
            {
                mensaje = "El año de fabricación no es válido.";
                return false;
            }

            if (unidad.CapacidadPasajeros <= 0)
            {
                mensaje = "La capacidad de pasajeros debe ser mayor a cero.";
                return false;
            }

            _unidadRepository.Agregar(unidad);

            mensaje = "Unidad agregada correctamente.";
            return true;
        }

        public bool Actualizar(Unidad unidad, out string mensaje)
        {
            var unidadActual = _unidadRepository.ObtenerPorId(unidad.Id);

            if (unidadActual == null)
            {
                mensaje = "Unidad no encontrada.";
                return false;
            }

            unidad.Placa = unidad.Placa.Trim().ToUpper();

            if (_unidadRepository.ExistePlaca(unidad.Placa, unidad.Id))
            {
                mensaje = "Ya existe otra unidad registrada con esa placa.";
                return false;
            }

            if (unidad.AnioFabricacion < 1980 || unidad.AnioFabricacion > DateTime.Now.Year + 1)
            {
                mensaje = "El año de fabricación no es válido.";
                return false;
            }

            if (unidad.CapacidadPasajeros <= 0)
            {
                mensaje = "La capacidad de pasajeros debe ser mayor a cero.";
                return false;
            }

            unidadActual.Placa = unidad.Placa;
            unidadActual.Modelo = unidad.Modelo;
            unidadActual.AnioFabricacion = unidad.AnioFabricacion;
            unidadActual.CapacidadPasajeros = unidad.CapacidadPasajeros;

            _unidadRepository.Actualizar(unidadActual);

            mensaje = "Unidad actualizada correctamente.";
            return true;
        }

        public bool Eliminar(int id, out string mensaje)
        {
            var unidad = _unidadRepository.ObtenerPorId(id);

            if (unidad == null)
            {
                mensaje = "Unidad no encontrada.";
                return false;
            }

            if (_unidadRepository.TieneViajes(id))
            {
                mensaje = "No se puede eliminar la unidad porque tiene viajes registrados.";
                return false;
            }

            _unidadRepository.Eliminar(unidad);

            mensaje = "Unidad eliminada correctamente.";
            return true;
        }
    }
}