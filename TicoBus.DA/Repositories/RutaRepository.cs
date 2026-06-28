using Microsoft.EntityFrameworkCore;
using TicoBus.DA.Data;
using TicoBus.Model;

namespace TicoBus.DA.Repositories
{
    public class RutaRepository
    {
        private readonly AppDbContext _context;

        public RutaRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Ruta> Listar(string? filtro)
        {
            var consulta = _context.Rutas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                consulta = consulta.Where(r =>
                    r.Nombre.Contains(filtro) ||
                    r.Destino.Contains(filtro));
            }

            return consulta.OrderBy(r => r.Nombre).ToList();
        }

        public Ruta? ObtenerPorId(int id)
        {
            return _context.Rutas.FirstOrDefault(r => r.Id == id);
        }

        public void Agregar(Ruta ruta)
        {
            _context.Rutas.Add(ruta);
            _context.SaveChanges();
        }

        public void Actualizar(Ruta ruta)
        {
            _context.Rutas.Update(ruta);
            _context.SaveChanges();
        }

        public bool TieneViajes(int rutaId)
        {
            return _context.Viajes.Any(v => v.RutaId == rutaId);
        }

        public void Eliminar(Ruta ruta)
        {
            _context.Rutas.Remove(ruta);
            _context.SaveChanges();
        }
    }
}