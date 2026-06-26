using Microsoft.EntityFrameworkCore;
using TicoBus.DA.Data;
using TicoBus.Model;

namespace TicoBus.DA.Repositories
{
    public class UsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public Usuario? ObtenerPorNombre(string nombre)
        {
            return _context.Usuarios
                .FirstOrDefault(u => u.Nombre == nombre && u.Activo);
        }

        public void Actualizar(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }
    }
}