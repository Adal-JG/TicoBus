using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.SI.DTOs;

namespace TicoBus.SI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadesController : ControllerBase
    {
        private readonly IUnidadService _unidadService;

        public UnidadesController(IUnidadService unidadService)
        {
            _unidadService = unidadService;
        }

        [HttpGet]
        public IActionResult Listar([FromQuery] string? filtro)
        {
            return Ok(new ApiResponse<List<Unidad>>
            {
                Exitoso = true,
                Datos = _unidadService.Listar(filtro)
            });
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id)
        {
            var unidad = _unidadService.ObtenerPorId(id);

            if (unidad == null)
            {
                return NotFound(new ApiResponse<Unidad>
                {
                    Exitoso = false,
                    Mensaje = "Unidad no encontrada."
                });
            }

            return Ok(new ApiResponse<Unidad>
            {
                Exitoso = true,
                Datos = unidad
            });
        }

        [HttpPost]
        public IActionResult Agregar([FromBody] Unidad unidad)
        {
            var resultado = _unidadService.Agregar(unidad, out string mensaje);

            if (!resultado)
            {
                return BadRequest(new ApiResponse<Unidad>
                {
                    Exitoso = false,
                    Mensaje = mensaje
                });
            }

            return Ok(new ApiResponse<Unidad>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Datos = unidad
            });
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Unidad unidad)
        {
            unidad.Id = id;

            var resultado = _unidadService.Actualizar(unidad, out string mensaje);

            if (!resultado)
            {
                return BadRequest(new ApiResponse<Unidad>
                {
                    Exitoso = false,
                    Mensaje = mensaje
                });
            }

            return Ok(new ApiResponse<Unidad>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Datos = unidad
            });
        }
    }
}