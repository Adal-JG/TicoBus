using Microsoft.EntityFrameworkCore;
using TicoBus.DA.Data;
using TicoBus.Model;

namespace TicoBus.DA.Repositories
{
    public class ReservaRepository
    {
        private readonly AppDbContext _context;

        public ReservaRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Reserva> ListarPorViaje(int viajeId)
        {
            return _context.Reservas
                .Include(r => r.Pasajero)
                .Where(r => r.ViajeId == viajeId && r.Activa)
                .OrderBy(r => r.NumeroAsiento)
                .ToList();
        }

        public bool AsientoOcupado(int viajeId, int numeroAsiento)
        {
            return _context.Reservas.Any(r =>
                r.ViajeId == viajeId &&
                r.NumeroAsiento == numeroAsiento &&
                r.Activa);
        }

        public int CantidadReservasActivas(int viajeId)
        {
            return _context.Reservas.Count(r =>
                r.ViajeId == viajeId &&
                r.Activa);
        }

        public decimal TotalRecaudado(int viajeId)
        {
            return _context.Reservas
                .Where(r => r.ViajeId == viajeId && r.Activa)
                .Sum(r => r.MontoPagado);
        }

        public Reserva? ObtenerPorId(int id)
        {
            return _context.Reservas
                .Include(r => r.Pasajero)
                .Include(r => r.Viaje)
                .FirstOrDefault(r => r.Id == id);
        }

        public void Agregar(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            _context.SaveChanges();
        }

        public void Actualizar(Reserva reserva)
        {
            _context.Reservas.Update(reserva);
            _context.SaveChanges();
        }
    }
}