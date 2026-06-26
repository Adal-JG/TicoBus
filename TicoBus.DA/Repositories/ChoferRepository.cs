using Microsoft.EntityFrameworkCore;
using TicoBus.DA.Data;
using TicoBus.Model;

namespace TicoBus.DA.Repositories
{
    public class ChoferRepository
    {
        private readonly AppDbContext _context;

        public ChoferRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Chofer> Listar(string? filtro)
        {
            var consulta = _context.Choferes
                .Include(c => c.Usuario)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                consulta = consulta.Where(c =>
                    c.Nombre.Contains(filtro) ||
                    c.Apellidos.Contains(filtro));
            }

            return consulta
                .OrderBy(c => c.Nombre)
                .ToList();
        }

        public Chofer? ObtenerPorId(int id)
        {
            return _context.Choferes
                .Include(c => c.Usuario)
                .FirstOrDefault(c => c.Id == id);
        }

        public bool ExisteIdentificacion(string identificacion, int idExcluir = 0)
        {
            return _context.Choferes.Any(c =>
                c.Identificacion == identificacion &&
                c.Id != idExcluir);
        }

        public void Agregar(Chofer chofer)
        {
            _context.Choferes.Add(chofer);
            _context.SaveChanges();
        }

        public void Actualizar(Chofer chofer)
        {
            _context.Choferes.Update(chofer);
            _context.SaveChanges();
        }

        public bool TieneViajes(int choferId)
        {
            return _context.Viajes.Any(v => v.ChoferId == choferId);
        }
    }
}