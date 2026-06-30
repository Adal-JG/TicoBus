using Microsoft.AspNetCore.Mvc;
using TicoBus.DA.Repositories;
using TicoBus.Model;
using TicoBus.SI.DTOs;

namespace TicoBus.SI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerfilController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;

        public PerfilController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id)
        {
            var usuario = _usuarioRepository.ObtenerPorId(id);

            if (usuario == null)
            {
                return NotFound(new ApiResponse<Usuario>
                {
                    Exitoso = false,
                    Mensaje = "Usuario no encontrado."
                });
            }

            return Ok(new ApiResponse<Usuario>
            {
                Exitoso = true,
                Datos = usuario
            });
        }

        [HttpPut("{id}/correo")]
        public IActionResult ActualizarCorreo(int id, [FromBody] ActualizarCorreoRequest request)
        {
            var usuario = _usuarioRepository.ObtenerPorId(id);

            if (usuario == null)
            {
                return NotFound(new ApiResponse<Usuario>
                {
                    Exitoso = false,
                    Mensaje = "Usuario no encontrado."
                });
            }

            usuario.Correo = request.Correo;

            _usuarioRepository.Actualizar(usuario);

            return Ok(new ApiResponse<Usuario>
            {
                Exitoso = true,
                Mensaje = "Perfil actualizado correctamente.",
                Datos = usuario
            });
        }
    }

    public class ActualizarCorreoRequest
    {
        public string Correo { get; set; } = string.Empty;
    }
}