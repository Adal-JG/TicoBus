using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.SI.DTOs;

namespace TicoBus.SI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasajerosController : ControllerBase
    {
        private readonly IPasajeroService _pasajeroService;

        public PasajerosController(IPasajeroService pasajeroService)
        {
            _pasajeroService = pasajeroService;
        }

        [HttpGet]
        public IActionResult Listar([FromQuery] string? filtro)
        {
            return Ok(new ApiResponse<List<Pasajero>>
            {
                Exitoso = true,
                Datos = _pasajeroService.Listar(filtro)
            });
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id)
        {
            var pasajero = _pasajeroService.ObtenerPorId(id);

            if (pasajero == null)
            {
                return NotFound(new ApiResponse<Pasajero>
                {
                    Exitoso = false,
                    Mensaje = "Pasajero no encontrado."
                });
            }

            return Ok(new ApiResponse<Pasajero>
            {
                Exitoso = true,
                Datos = pasajero
            });
        }

        [HttpPost]
        public IActionResult Agregar([FromBody] Pasajero pasajero)
        {
            var resultado = _pasajeroService.Agregar(pasajero, out string mensaje);

            if (!resultado)
            {
                return BadRequest(new ApiResponse<Pasajero>
                {
                    Exitoso = false,
                    Mensaje = mensaje
                });
            }

            return Ok(new ApiResponse<Pasajero>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Datos = pasajero
            });
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Pasajero pasajero)
        {
            pasajero.Id = id;

            var resultado = _pasajeroService.Actualizar(pasajero, out string mensaje);

            if (!resultado)
            {
                return BadRequest(new ApiResponse<Pasajero>
                {
                    Exitoso = false,
                    Mensaje = mensaje
                });
            }

            return Ok(new ApiResponse<Pasajero>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Datos = pasajero
            });
        }
    }
}