using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.SI.DTOs;

namespace TicoBus.SI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViajesController : ControllerBase
    {
        private readonly IViajeService _viajeService;

        public ViajesController(IViajeService viajeService)
        {
            _viajeService = viajeService;
        }

        [HttpGet]
        public IActionResult Listar(string? filtro)
        {
            return Ok(new ApiResponse<List<Viaje>>
            {
                Exitoso = true,
                Datos = _viajeService.Listar(filtro)
            });
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id)
        {
            var viaje = _viajeService.ObtenerPorId(id);

            if (viaje == null)
            {
                return NotFound(new ApiResponse<Viaje>
                {
                    Exitoso = false,
                    Mensaje = "Viaje no encontrado."
                });
            }

            return Ok(new ApiResponse<Viaje>
            {
                Exitoso = true,
                Datos = viaje
            });
        }

        [HttpPost]
        public IActionResult Crear(Viaje viaje)
        {
            var ok = _viajeService.Agregar(viaje, out string mensaje);

            if (!ok)
            {
                return BadRequest(new ApiResponse<Viaje>
                {
                    Exitoso = false,
                    Mensaje = mensaje
                });
            }

            return Ok(new ApiResponse<Viaje>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Datos = viaje
            });
        }

        [HttpPut("{id}")]
        public IActionResult Editar(int id, Viaje viaje)
        {
            viaje.Id = id;

            var ok = _viajeService.Actualizar(viaje, out string mensaje);

            if (!ok)
            {
                return BadRequest(new ApiResponse<Viaje>
                {
                    Exitoso = false,
                    Mensaje = mensaje
                });
            }

            return Ok(new ApiResponse<Viaje>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Datos = viaje
            });
        }

        [HttpPost("{id}/iniciar")]
        public IActionResult Iniciar(int id)
        {
            var ok = _viajeService.Iniciar(id, out string mensaje);

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

        [HttpPost("{id}/finalizar")]
        public IActionResult Finalizar(int id)
        {
            var ok = _viajeService.Finalizar(id, out string mensaje);

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

        [HttpPost("{id}/cancelar")]
        public IActionResult Cancelar(int id, [FromBody] string motivo)
        {
            var ok = _viajeService.Cancelar(id, motivo, out string mensaje);

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