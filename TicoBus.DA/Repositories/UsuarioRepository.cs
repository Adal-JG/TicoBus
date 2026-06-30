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

        public Usuario? ObtenerPorNombreUsuario(string nombreUsuario)
        {
            return _context.Usuarios
                .FirstOrDefault(u => u.NombreUsuario == nombreUsuario && u.Activo);
        }

        public bool ExisteNombreUsuario(string nombreUsuario)
        {
            return _context.Usuarios.Any(u => u.NombreUsuario == nombreUsuario);
        }
        public Usuario? ObtenerPorId(int id)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Id == id);
        }
        public void Actualizar(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }
        public void Eliminar(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }
    }
}