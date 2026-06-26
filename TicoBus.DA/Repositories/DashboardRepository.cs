using TicoBus.DA.Data;
using TicoBus.Model;

namespace TicoBus.DA.Repositories
{
    public class DashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public int TotalChoferes()
        {
            return _context.Choferes.Count();
        }

        public int TotalPasajeros()
        {
            return _context.Pasajeros.Count();
        }

        public int TotalRutas()
        {
            return _context.Rutas.Count();
        }

        public int TotalUnidades()
        {
            return _context.Unidades.Count();
        }

        public int TotalViajesProgramados()
        {
            return _context.Viajes.Count(v => v.Estado == EstadoViaje.Programado);
        }

        public int TotalViajesEnCurso()
        {
            return _context.Viajes.Count(v => v.Estado == EstadoViaje.EnCurso);
        }

        public int TotalViajesCancelados()
        {
            return _context.Viajes.Count(v => v.Estado == EstadoViaje.Cancelado);
        }
    }
}