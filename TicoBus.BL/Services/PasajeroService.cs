using TicoBus.BL.Helpers;
using TicoBus.BL.Interfaces;
using TicoBus.DA.Repositories;
using TicoBus.Model;

namespace TicoBus.BL.Services
{
    public class PasajeroService : IPasajeroService
    {
        private readonly PasajeroRepository _pasajeroRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly CorreoService _correoService;

        public PasajeroService(
            PasajeroRepository pasajeroRepository,
            UsuarioRepository usuarioRepository,
            CorreoService correoService)
        {
            _pasajeroRepository = pasajeroRepository;
            _usuarioRepository = usuarioRepository;
            _correoService = correoService;
        }

        public List<Pasajero> Listar(string? filtro)
        {
            return _pasajeroRepository.Listar(filtro);
        }
        public Pasajero? ObtenerPorUsuarioId(int usuarioId)
        {
            return _pasajeroRepository.ObtenerPorUsuarioId(usuarioId);
        }

        public Pasajero? ObtenerPorId(int id)
        {
            return _pasajeroRepository.ObtenerPorId(id);
        }

        public bool Agregar(Pasajero pasajero, out string mensaje)
        {
            if (_pasajeroRepository.ExisteIdentificacion(pasajero.Identificacion))
            {
                mensaje = "Ya existe un pasajero con esa identificación.";
                return false;
            }

            var nombreUsuario = GenerarNombreUsuario(pasajero.Nombre, pasajero.Apellidos);
            var claveAleatoria = GeneradorClave.Generar();

            pasajero.Usuario = new Usuario
            {
                NombreUsuario = nombreUsuario,
                NombreCompleto = $"{pasajero.Nombre} {pasajero.Apellidos}",
                Clave = claveAleatoria,
                Correo = pasajero.Correo,
                Rol = RolUsuario.Pasajero,
                Activo = true,
                IntentosFallidos = 0
            };

            _pasajeroRepository.Agregar(pasajero);

            var cuerpo =
                $"Hola {pasajero.Nombre},\n\n" +
                $"Su usuario de acceso al sistema TicoBus fue creado correctamente.\n\n" +
                $"Usuario: {nombreUsuario}\n" +
                $"Clave temporal: {claveAleatoria}\n\n" +
                $"Se recomienda cambiar la clave al iniciar sesión.";

            _correoService.EnviarCorreo(
                pasajero.Correo,
                "Usuario creado — TicoBus",
                cuerpo);

            mensaje = "Pasajero agregado correctamente. Se envió la clave al correo.";
            return true;
        }

        public bool Actualizar(Pasajero pasajero, out string mensaje)
        {
            var pasajeroActual = _pasajeroRepository.ObtenerPorId(pasajero.Id);

            if (pasajeroActual == null)
            {
                mensaje = "Pasajero no encontrado.";
                return false;
            }

            if (_pasajeroRepository.ExisteIdentificacion(pasajero.Identificacion, pasajero.Id))
            {
                mensaje = "Ya existe otro pasajero con esa identificación.";
                return false;
            }

            pasajeroActual.Identificacion = pasajero.Identificacion;
            pasajeroActual.Nombre = pasajero.Nombre;
            pasajeroActual.Apellidos = pasajero.Apellidos;
            pasajeroActual.Correo = pasajero.Correo;

            if (pasajeroActual.Usuario != null)
            {
                pasajeroActual.Usuario.NombreCompleto = $"{pasajero.Nombre} {pasajero.Apellidos}";
                pasajeroActual.Usuario.Correo = pasajero.Correo;
            }

            _pasajeroRepository.Actualizar(pasajeroActual);

            mensaje = "Pasajero actualizado correctamente.";
            return true;
        }

        public bool Eliminar(int id, out string mensaje)
        {
            var pasajero = _pasajeroRepository.ObtenerPorId(id);

            if (pasajero == null)
            {
                mensaje = "Pasajero no encontrado.";
                return false;
            }

            var usuario = pasajero.Usuario;

            _pasajeroRepository.Eliminar(pasajero);

            if (usuario != null)
            {
                _usuarioRepository.Eliminar(usuario);
            }

            mensaje = "Pasajero eliminado correctamente.";
            return true;
        }

        private string GenerarNombreUsuario(string nombre, string apellidos)
        {
            var primerNombre = nombre.Trim().Split(' ')[0].ToLower();
            var primerApellido = apellidos.Trim().Split(' ')[0].ToLower();

            var baseUsuario = $"{primerNombre[0]}{primerApellido}";
            var usuarioFinal = baseUsuario;
            var contador = 1;

            while (_usuarioRepository.ExisteNombreUsuario(usuarioFinal))
            {
                usuarioFinal = $"{baseUsuario}{contador}";
                contador++;
            }

            return usuarioFinal;
        }
    }
}