using TicoBus.BL.Helpers;
using TicoBus.BL.Interfaces;
using TicoBus.DA.Repositories;
using TicoBus.Model;

namespace TicoBus.BL.Services
{
    public class ChoferService : IChoferService
    {
        private readonly ChoferRepository _choferRepository;
        private readonly CorreoService _correoService;

        public ChoferService(ChoferRepository choferRepository, CorreoService correoService)
        {
            _choferRepository = choferRepository;
            _correoService = correoService;
        }

        public List<Chofer> Listar(string? filtro)
        {
            return _choferRepository.Listar(filtro);
        }

        public Chofer? ObtenerPorId(int id)
        {
            return _choferRepository.ObtenerPorId(id);
        }

        public bool Agregar(Chofer chofer, out string mensaje)
        {
            if (_choferRepository.ExisteIdentificacion(chofer.Identificacion))
            {
                mensaje = "Ya existe un chofer con esa identificación.";
                return false;
            }

            var claveAleatoria = GeneradorClave.Generar();

            chofer.Usuario = new Usuario
            {
                Nombre = chofer.Identificacion,
                Clave = claveAleatoria,
                Correo = chofer.Correo,
                Rol = RolUsuario.Chofer,
                Activo = true,
                IntentosFallidos = 0
            };

            _choferRepository.Agregar(chofer);

            var cuerpo =
                $"Hola {chofer.Nombre},\n\n" +
                $"Su usuario de acceso al sistema TicoBus fue creado correctamente.\n\n" +
                $"Usuario: {chofer.Identificacion}\n" +
                $"Clave temporal: {claveAleatoria}\n\n" +
                $"Se recomienda cambiar la clave al iniciar sesión.";

            _correoService.EnviarCorreo(
                chofer.Correo,
                "Usuario creado — TicoBus",
                cuerpo);

            mensaje = "Chofer agregado correctamente. Se envió la clave al correo.";
            return true;
        }

        public bool Actualizar(Chofer chofer, out string mensaje)
        {
            var choferActual = _choferRepository.ObtenerPorId(chofer.Id);

            if (choferActual == null)
            {
                mensaje = "Chofer no encontrado.";
                return false;
            }

            if (_choferRepository.ExisteIdentificacion(chofer.Identificacion, chofer.Id))
            {
                mensaje = "Ya existe otro chofer con esa identificación.";
                return false;
            }

            choferActual.Identificacion = chofer.Identificacion;
            choferActual.Nombre = chofer.Nombre;
            choferActual.Apellidos = chofer.Apellidos;
            choferActual.Correo = chofer.Correo;

            if (choferActual.Usuario != null)
            {
                choferActual.Usuario.Nombre = chofer.Identificacion;
                choferActual.Usuario.Correo = chofer.Correo;
            }

            _choferRepository.Actualizar(choferActual);

            mensaje = "Chofer actualizado correctamente.";
            return true;
        }

        public bool TieneViajes(int choferId)
        {
            return _choferRepository.TieneViajes(choferId);
        }
    }
}