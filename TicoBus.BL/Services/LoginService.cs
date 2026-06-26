using TicoBus.BL.Interfaces;
using TicoBus.DA.Repositories;
using TicoBus.Model;

namespace TicoBus.BL.Services
{
    public class LoginService : ILoginService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly CorreoService _correoService;

        public LoginService(UsuarioRepository usuarioRepository, CorreoService correoService)
        {
            _usuarioRepository = usuarioRepository;
            _correoService = correoService;
        }

        public Usuario? Login(string nombre, string clave, out string mensaje)
        {
            mensaje = string.Empty;

            var usuario = _usuarioRepository.ObtenerPorNombre(nombre);

            if (usuario == null)
            {
                mensaje = "Usuario no encontrado.";
                return null;
            }

            if (usuario.Rol != RolUsuario.Administrador &&
                usuario.BloqueadoHasta != null &&
                usuario.BloqueadoHasta > DateTime.Now)
            {
                var cuerpoBloqueado =
                    $"La cuenta {usuario.Nombre} está bloqueada por 3 minutos. " +
                    $"Puede reintentar el {usuario.BloqueadoHasta.Value:dd/MM/yyyy} a las {usuario.BloqueadoHasta.Value:HH:mm}.";

                _correoService.EnviarCorreo(usuario.Correo, "Cuenta bloqueada", cuerpoBloqueado);

                mensaje = cuerpoBloqueado;
                return null;
            }

            if (usuario.Clave != clave)
            {
                if (usuario.Rol != RolUsuario.Administrador)
                {
                    usuario.IntentosFallidos++;

                    if (usuario.IntentosFallidos >= 2)
                    {
                        usuario.BloqueadoHasta = DateTime.Now.AddMinutes(3);
                        usuario.IntentosFallidos = 0;

                        var cuerpo =
                            $"La cuenta {usuario.Nombre} está bloqueada por 3 minutos. " +
                            $"Puede reintentar el {usuario.BloqueadoHasta.Value:dd/MM/yyyy} a las {usuario.BloqueadoHasta.Value:HH:mm}.";

                        _correoService.EnviarCorreo(usuario.Correo, "Cuenta bloqueada", cuerpo);
                    }

                    _usuarioRepository.Actualizar(usuario);
                }

                mensaje = "Nombre o clave incorrectos.";
                return null;
            }

            usuario.IntentosFallidos = 0;
            usuario.BloqueadoHasta = null;
            _usuarioRepository.Actualizar(usuario);

            var cuerpoInicio =
                $"Usted inició sesión el día {DateTime.Now:dd/MM/yyyy} a las {DateTime.Now:HH:mm}.";

            _correoService.EnviarCorreo(
                usuario.Correo,
                $"Inicio de sesión — {usuario.Nombre}",
                cuerpoInicio);

            mensaje = "Inicio de sesión correcto.";
            return usuario;
        }

        public bool CambiarClave(string nombre, string claveActual, string nuevaClave, out string mensaje)
        {
            var usuario = _usuarioRepository.ObtenerPorNombre(nombre);

            if (usuario == null)
            {
                mensaje = "Usuario no encontrado.";
                return false;
            }

            if (usuario.Clave != claveActual)
            {
                mensaje = "La clave actual no es correcta.";
                return false;
            }

            usuario.Clave = nuevaClave;
            _usuarioRepository.Actualizar(usuario);

            var cuerpo =
                $"La clave de su cuenta fue actualizada el día {DateTime.Now:dd/MM/yyyy} a las {DateTime.Now:HH:mm}.";

            _correoService.EnviarCorreo(
                usuario.Correo,
                $"Cambio de clave — {usuario.Nombre}",
                cuerpo);

            mensaje = "Clave actualizada correctamente.";
            return true;
        }
    }
}
