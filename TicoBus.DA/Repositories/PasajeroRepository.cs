using TicoBus.DA.Data;
using TicoBus.Model;

using Microsoft.EntityFrameworkCore;
using TicoBus.DA.Data;
using TicoBus.Model;

namespace TicoBus.DA.Repositories
{
    public class PasajeroRepository
    {
        private readonly AppDbContext _context;

        public PasajeroRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Pasajero> Listar(string? filtro)
        {
            var consulta = _context.Pasajeros
                .Include(p => p.Usuario)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                consulta = consulta.Where(p =>
                    p.Nombre.Contains(filtro) ||
                    p.Apellidos.Contains(filtro));
            }

            return consulta.OrderBy(p => p.Nombre).ToList();
        }

        public Pasajero? ObtenerPorId(int id)
        {
            return _context.Pasajeros
                .Include(p => p.Usuario)
                .FirstOrDefault(p => p.Id == id);
        }
        public Pasajero? ObtenerPorUsuarioId(int usuarioId)
        {
            return _context.Pasajeros
                .Include(p => p.Usuario)
                .FirstOrDefault(p => p.UsuarioId == usuarioId);
        }

        public bool ExisteIdentificacion(string identificacion, int idExcluir = 0)
        {
            return _context.Pasajeros.Any(p =>
                p.Identificacion == identificacion &&
                p.Id != idExcluir);
        }

        public void Agregar(Pasajero pasajero)
        {
            _context.Pasajeros.Add(pasajero);
            _context.SaveChanges();
        }

        public void Actualizar(Pasajero pasajero)
        {
            _context.Pasajeros.Update(pasajero);
            _context.SaveChanges();
        }

        public void Eliminar(Pasajero pasajero)
        {
            _context.Pasajeros.Remove(pasajero);
            _context.SaveChanges();
        }
    }
}