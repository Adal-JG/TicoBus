using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.SI.DTOs;

namespace TicoBus.SI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChoferesController : ControllerBase
    {
        private readonly IChoferService _choferService;

        public ChoferesController(IChoferService choferService)
        {
            _choferService = choferService;
        }

        [HttpGet]
        public IActionResult Listar([FromQuery] string? filtro)
        {
            var choferes = _choferService.Listar(filtro);

            return Ok(new ApiResponse<List<Chofer>>
            {
                Exitoso = true,
                Datos = choferes
            });
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id)
        {
            var chofer = _choferService.ObtenerPorId(id);

            if (chofer == null)
            {
                return NotFound(new ApiResponse<Chofer>
                {
                    Exitoso = false,
                    Mensaje = "Chofer no encontrado."
                });
            }

            return Ok(new ApiResponse<Chofer>
            {
                Exitoso = true,
                Datos = chofer
            });
        }

        [HttpPost]
        public IActionResult Agregar([FromBody] Chofer chofer)
        {
            var resultado = _choferService.Agregar(chofer, out string mensaje);

            if (!resultado)
            {
                return BadRequest(new ApiResponse<Chofer>
                {
                    Exitoso = false,
                    Mensaje = mensaje
                });
            }

            return Ok(new ApiResponse<Chofer>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Datos = chofer
            });
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Chofer chofer)
        {
            chofer.Id = id;

            var resultado = _choferService.Actualizar(chofer, out string mensaje);

            if (!resultado)
            {
                return BadRequest(new ApiResponse<Chofer>
                {
                    Exitoso = false,
                    Mensaje = mensaje
                });
            }

            return Ok(new ApiResponse<Chofer>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Datos = chofer
            });
        }
    }
}