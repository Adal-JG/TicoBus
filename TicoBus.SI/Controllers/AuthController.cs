using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.SI.DTOs;

namespace TicoBus.SI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var usuario = _loginService.Login(request.NombreUsuario, request.Clave, out string mensaje);

            if (usuario == null)
            {
                return Unauthorized(new ApiResponse<LoginResponse>
                {
                    Exitoso = false,
                    Mensaje = mensaje
                });
            }

            var datos = new LoginResponse
            {
                Exitoso = true,
                Mensaje = "Inicio de sesión correcto.",
                UsuarioId = usuario.Id,
                Nombre = usuario.NombreCompleto,
                Rol = usuario.Rol.ToString()
            };

            return Ok(new ApiResponse<LoginResponse>
            {
                Exitoso = true,
                Mensaje = "Inicio de sesión correcto.",
                Datos = datos
            });
        }
        
        [HttpPost("cambiar-clave")]
        public IActionResult CambiarClave(CambiarClaveRequest request)
        {
            var ok = _loginService.CambiarClave(
                request.NombreUsuario,
                request.ClaveActual,
                request.NuevaClave,
                out string mensaje);

            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Exitoso = false,
                    Mensaje = mensaje
                });
            }

            return Ok(new ApiResponse<object>
            {
                Exitoso = true,
                Mensaje = mensaje
            });
        }
    }
}