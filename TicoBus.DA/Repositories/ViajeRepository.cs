using Microsoft.EntityFrameworkCore;
using TicoBus.DA.Data;
using TicoBus.Model;

namespace TicoBus.DA.Repositories
{
    public class ViajeRepository
    {
        private readonly AppDbContext _context;

        public ViajeRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Viaje> Listar(string? filtro)
        {
            var consulta = _context.Viajes
                .Include(v => v.Ruta)
                .Include(v => v.Unidad)
                .Include(v => v.Chofer)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                consulta = consulta.Where(v =>
                    v.Ruta!.Nombre.Contains(filtro) ||
                    v.Ruta.Destino.Contains(filtro) ||
                    v.FechaHoraSalida.ToString().Contains(filtro));
            }

            return consulta
                .OrderByDescending(v => v.FechaHoraSalida)
                .ToList();
        }

        public List<Viaje> ListarCancelados()
        {
            return _context.Viajes
                .Include(v => v.Ruta)
                .Include(v => v.Unidad)
                .Include(v => v.Chofer)
                .Where(v => v.Estado == EstadoViaje.Cancelado)
                .OrderByDescending(v => v.FechaHoraSalida)
                .ToList();
        }

        public Viaje? ObtenerPorId(int id)
        {
            return _context.Viajes
                .Include(v => v.Ruta)
                .Include(v => v.Unidad)
                .Include(v => v.Chofer)
                .Include(v => v.Reservas)
                    .ThenInclude(r => r.Pasajero)
                .FirstOrDefault(v => v.Id == id);
        }

        public bool ChoferOcupado(int choferId, DateTime salida, DateTime llegada, int viajeIdExcluir = 0)
        {
            return _context.Viajes.Any(v =>
                v.Id != viajeIdExcluir &&
                v.ChoferId == choferId &&
                (v.Estado == EstadoViaje.Programado || v.Estado == EstadoViaje.EnCurso) &&
                salida < v.FechaHoraLlegadaEstimada &&
                llegada > v.FechaHoraSalida);
        }

        public bool UnidadOcupada(int unidadId, DateTime salida, DateTime llegada, int viajeIdExcluir = 0)
        {
            return _context.Viajes.Any(v =>
                v.Id != viajeIdExcluir &&
                v.UnidadId == unidadId &&
                (v.Estado == EstadoViaje.Programado || v.Estado == EstadoViaje.EnCurso) &&
                salida < v.FechaHoraLlegadaEstimada &&
                llegada > v.FechaHoraSalida);
        }

        public void Agregar(Viaje viaje)
        {
            _context.Viajes.Add(viaje);
            _context.SaveChanges();
        }

        public void Actualizar(Viaje viaje)
        {
            _context.Viajes.Update(viaje);
            _context.SaveChanges();
        }
        public List<Viaje> ListarEnCurso()
        {
            return _context.Viajes
                .Include(v => v.Ruta)
                .Include(v => v.Unidad)
                .Include(v => v.Chofer)
                .Include(v => v.Reservas)
                .Where(v => v.Estado == EstadoViaje.EnCurso)
                .OrderByDescending(v => v.FechaHoraSalida)
                .ToList();
        }

        public void Finalizar(Viaje viaje)
        {
            viaje.Estado = EstadoViaje.Completado;
            _context.Viajes.Update(viaje);
            _context.SaveChanges();
        }
    }
}