using TicoBus.DA.Data;
using TicoBus.Model;

namespace TicoBus.DA.Repositories
{
    public class UnidadRepository
    {
        private readonly AppDbContext _context;

        public UnidadRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Unidad> Listar(string? filtro)
        {
            var consulta = _context.Unidades.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                consulta = consulta.Where(u =>
                    u.Placa.Contains(filtro) ||
                    u.Modelo.Contains(filtro));
            }

            return consulta.OrderBy(u => u.Placa).ToList();
        }

        public Unidad? ObtenerPorId(int id)
        {
            return _context.Unidades.FirstOrDefault(u => u.Id == id);
        }

        public bool ExistePlaca(string placa, int idExcluir = 0)
        {
            return _context.Unidades.Any(u =>
                u.Placa == placa &&
                u.Id != idExcluir);
        }

        public void Agregar(Unidad unidad)
        {
            _context.Unidades.Add(unidad);
            _context.SaveChanges();
        }

        public void Actualizar(Unidad unidad)
        {
            _context.Unidades.Update(unidad);
            _context.SaveChanges();
        }

        public bool TieneViajes(int unidadId)
        {
            return _context.Viajes.Any(v => v.UnidadId == unidadId);
        }

        public void Eliminar(Unidad unidad)
        {
            _context.Unidades.Remove(unidad);
            _context.SaveChanges();
        }
    }
}