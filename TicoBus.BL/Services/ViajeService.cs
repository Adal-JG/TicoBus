using TicoBus.BL.Interfaces;
using TicoBus.DA.Repositories;
using TicoBus.Model;

namespace TicoBus.BL.Services
{
    public class ViajeService : IViajeService
    {
        private readonly ViajeRepository _viajeRepository;

        public List<Viaje> ListarEnCurso()
        {
            return _viajeRepository.ListarEnCurso();
        }

        public bool Finalizar(int id, out string mensaje)
        {
            var viaje = _viajeRepository.ObtenerPorId(id);

            if (viaje == null)
            {
                mensaje = "Viaje no encontrado.";
                return false;
            }

            if (viaje.Estado != EstadoViaje.EnCurso)
            {
                mensaje = "Solo se pueden finalizar viajes en estado En Curso.";
                return false;
            }

            _viajeRepository.Finalizar(viaje);

            mensaje = "Viaje finalizado correctamente.";
            return true;
        }

        public ViajeService(ViajeRepository viajeRepository)
        {
            _viajeRepository = viajeRepository;
        }
        public List<Viaje> ListarCancelados()
        {
            return _viajeRepository.ListarCancelados();
        }
        public List<Viaje> Listar(string? filtro)
        {
            return _viajeRepository.Listar(filtro);
        }

        public Viaje? ObtenerPorId(int id)
        {
            return _viajeRepository.ObtenerPorId(id);
        }
        public List<Viaje> ListarPorChoferHoy(int choferId)
        {
            return _viajeRepository.ListarPorChoferHoy(choferId);
        }

        public Viaje? ObtenerProximoViajeChofer(int choferId)
        {
            return _viajeRepository.ObtenerProximoViajeChofer(choferId);
        }
        public bool Agregar(Viaje viaje, out string mensaje)
        {
            if (viaje.FechaHoraLlegadaEstimada <= viaje.FechaHoraSalida)
            {
                mensaje = "La fecha y hora estimada de llegada debe ser posterior a la salida.";
                return false;
            }

            if (_viajeRepository.ChoferOcupado(viaje.ChoferId, viaje.FechaHoraSalida, viaje.FechaHoraLlegadaEstimada))
            {
                mensaje = "El chofer ya tiene un viaje activo en ese rango de fechas.";
                return false;
            }

            if (_viajeRepository.UnidadOcupada(viaje.UnidadId, viaje.FechaHoraSalida, viaje.FechaHoraLlegadaEstimada))
            {
                mensaje = "La unidad ya tiene un viaje activo en ese rango de fechas.";
                return false;
            }

            viaje.Estado = EstadoViaje.Programado;
            _viajeRepository.Agregar(viaje);

            mensaje = "Viaje registrado correctamente.";
            return true;
        }

        public bool Actualizar(Viaje viaje, out string mensaje)
        {
            var viajeActual = _viajeRepository.ObtenerPorId(viaje.Id);

            if (viajeActual == null)
            {
                mensaje = "Viaje no encontrado.";
                return false;
            }

            if (viajeActual.Estado != EstadoViaje.Programado)
            {
                mensaje = "Solo se pueden editar viajes en estado Programado.";
                return false;
            }

            if (viaje.FechaHoraLlegadaEstimada <= viaje.FechaHoraSalida)
            {
                mensaje = "La fecha y hora estimada de llegada debe ser posterior a la salida.";
                return false;
            }

            if (_viajeRepository.ChoferOcupado(viaje.ChoferId, viaje.FechaHoraSalida, viaje.FechaHoraLlegadaEstimada, viaje.Id))
            {
                mensaje = "El chofer ya tiene un viaje activo en ese rango de fechas.";
                return false;
            }

            if (_viajeRepository.UnidadOcupada(viaje.UnidadId, viaje.FechaHoraSalida, viaje.FechaHoraLlegadaEstimada, viaje.Id))
            {
                mensaje = "La unidad ya tiene un viaje activo en ese rango de fechas.";
                return false;
            }

            viajeActual.RutaId = viaje.RutaId;
            viajeActual.UnidadId = viaje.UnidadId;
            viajeActual.ChoferId = viaje.ChoferId;
            viajeActual.FechaHoraSalida = viaje.FechaHoraSalida;
            viajeActual.FechaHoraLlegadaEstimada = viaje.FechaHoraLlegadaEstimada;

            _viajeRepository.Actualizar(viajeActual);

            mensaje = "Viaje actualizado correctamente.";
            return true;
        }

        public bool Iniciar(int id, out string mensaje)
        {
            var viaje = _viajeRepository.ObtenerPorId(id);

            if (viaje == null)
            {
                mensaje = "Viaje no encontrado.";
                return false;
            }

            if (viaje.Estado != EstadoViaje.Programado)
            {
                mensaje = "Solo se pueden iniciar viajes en estado Programado.";
                return false;
            }

            viaje.Estado = EstadoViaje.EnCurso;
            _viajeRepository.Actualizar(viaje);

            mensaje = "Viaje iniciado correctamente.";
            return true;
        }

        public bool Cancelar(int id, string motivo, out string mensaje)
        {
            var viaje = _viajeRepository.ObtenerPorId(id);

            if (viaje == null)
            {
                mensaje = "Viaje no encontrado.";
                return false;
            }

            if (viaje.Estado != EstadoViaje.Programado)
            {
                mensaje = "Solo se pueden cancelar viajes en estado Programado.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(motivo))
            {
                mensaje = "Debe indicar un motivo de cancelación.";
                return false;
            }

            viaje.Estado = EstadoViaje.Cancelado;
            viaje.MotivoCancelacion = motivo;

            _viajeRepository.Actualizar(viaje);

            mensaje = "Viaje cancelado correctamente.";
            return true;
        }
    }
}