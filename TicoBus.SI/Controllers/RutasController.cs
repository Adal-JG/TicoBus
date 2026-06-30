using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.SI.DTOs;

namespace TicoBus.SI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RutasController : ControllerBase
    {
        private readonly IRutaService _rutaService;

        public RutasController(IRutaService rutaService)
        {
            _rutaService = rutaService;
        }

        [HttpGet]
        public IActionResult Listar([FromQuery] string? filtro)
        {
            return Ok(new ApiResponse<List<Ruta>>
            {
                Exitoso = true,
                Datos = _rutaService.Listar(filtro)
            });
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id)
        {
            var ruta = _rutaService.ObtenerPorId(id);

            if (ruta == null)
            {
                return NotFound(new ApiResponse<Ruta>
                {
                    Exitoso = false,
                    Mensaje = "Ruta no encontrada."
                });
            }

            return Ok(new ApiResponse<Ruta>
            {
                Exitoso = true,
                Datos = ruta
            });
        }

        [HttpPost]
        public IActionResult Agregar([FromBody] Ruta ruta)
        {
            var resultado = _rutaService.Agregar(ruta, out string mensaje);

            if (!resultado)
            {
                return BadRequest(new ApiResponse<Ruta>
                {
                    Exitoso = false,
                    Mensaje = mensaje
                });
            }

            return Ok(new ApiResponse<Ruta>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Datos = ruta
            });
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Ruta ruta)
        {
            ruta.Id = id;

            var resultado = _rutaService.Actualizar(ruta, out string mensaje);

            if (!resultado)
            {
                return BadRequest(new ApiResponse<Ruta>
                {
                    Exitoso = false,
                    Mensaje = mensaje
                });
            }

            return Ok(new ApiResponse<Ruta>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Datos = ruta
            });
        }
    }
}