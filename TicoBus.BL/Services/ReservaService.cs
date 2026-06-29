using TicoBus.BL.Interfaces;
using TicoBus.DA.Repositories;
using TicoBus.Model;

namespace TicoBus.BL.Services
{
    public class ReservaService : IReservaService
    {
        private readonly ReservaRepository _reservaRepository;
        private readonly ViajeRepository _viajeRepository;

        public ReservaService(
            ReservaRepository reservaRepository,
            ViajeRepository viajeRepository)
        {
            _reservaRepository = reservaRepository;
            _viajeRepository = viajeRepository;
        }

        public List<Reserva> ListarPorViaje(int viajeId)
        {
            return _reservaRepository.ListarPorViaje(viajeId);
        }

        public bool Reservar(int viajeId, int pasajeroId, int numeroAsiento, out string mensaje)
        {
            var viaje = _viajeRepository.ObtenerPorId(viajeId);

            if (viaje == null)
            {
                mensaje = "Viaje no encontrado.";
                return false;
            }

            if (viaje.Estado != EstadoViaje.EnCurso)
            {
                mensaje = "Solo se pueden reservar asientos en viajes en curso.";
                return false;
            }

            if (numeroAsiento <= 0)
            {
                mensaje = "El número de asiento debe ser mayor a cero.";
                return false;
            }

            if (_reservaRepository.AsientoOcupado(viajeId, numeroAsiento))
            {
                mensaje = "El asiento ya está ocupado en este viaje.";
                return false;
            }

            var ocupados = _reservaRepository.CantidadReservasActivas(viajeId);

            if (viaje.Unidad == null)
            {
                mensaje = "La unidad del viaje no está disponible.";
                return false;
            }

            if (ocupados >= viaje.Unidad.CapacidadPasajeros)
            {
                mensaje = "No hay asientos disponibles en esta unidad.";
                return false;
            }

            if (numeroAsiento > viaje.Unidad.CapacidadPasajeros)
            {
                mensaje = $"El número de asiento no puede superar la capacidad de la unidad ({viaje.Unidad.CapacidadPasajeros}).";
                return false;
            }

            if (viaje.Ruta == null)
            {
                mensaje = "La ruta del viaje no está disponible.";
                return false;
            }

            var reserva = new Reserva
            {
                ViajeId = viajeId,
                PasajeroId = pasajeroId,
                NumeroAsiento = numeroAsiento,
                MontoPagado = viaje.Ruta.PrecioBase,
                Activa = true,
                FechaReserva = DateTime.Now
            };

            _reservaRepository.Agregar(reserva);

            mensaje = "Reserva registrada correctamente.";
            return true;
        }

        public bool CancelarReserva(int reservaId, out string mensaje)
        {
            var reserva = _reservaRepository.ObtenerPorId(reservaId);

            if (reserva == null)
            {
                mensaje = "Reserva no encontrada.";
                return false;
            }

            if (reserva.Viaje != null && reserva.Viaje.Estado == EstadoViaje.Completado)
            {
                mensaje = "No se puede cancelar una reserva de un viaje completado.";
                return false;
            }

            reserva.Activa = false;
            _reservaRepository.Actualizar(reserva);

            mensaje = "Reserva cancelada correctamente.";
            return true;
        }

        public int CantidadReservasActivas(int viajeId)
        {
            return _reservaRepository.CantidadReservasActivas(viajeId);
        }

        public decimal TotalRecaudado(int viajeId)
        {
            return _reservaRepository.TotalRecaudado(viajeId);
        }
    }
}