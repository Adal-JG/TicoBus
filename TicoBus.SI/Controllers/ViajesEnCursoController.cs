using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.SI.DTOs;

namespace TicoBus.SI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViajesEnCursoController : ControllerBase
    {
        private readonly IViajeService _viajeService;
        private readonly IReservaService _reservaService;

        public ViajesEnCursoController(
            IViajeService viajeService,
            IReservaService reservaService)
        {
            _viajeService = viajeService;
            _reservaService = reservaService;
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(new ApiResponse<List<Viaje>>
            {
                Exitoso = true,
                Datos = _viajeService.ListarEnCurso()
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

        [HttpGet("{id}/reservas")]
        public IActionResult Reservas(int id)
        {
            return Ok(new ApiResponse<List<Reserva>>
            {
                Exitoso = true,
                Datos = _reservaService.ListarPorViaje(id)
            });
        }

        [HttpGet("{id}/totales")]
        public IActionResult Totales(int id)
        {
            var cantidad = _reservaService.CantidadReservasActivas(id);
            var total = _reservaService.TotalRecaudado(id);

            return Ok(new ApiResponse<object>
            {
                Exitoso = true,
                Datos = new
                {
                    pasajeros = cantidad,
                    totalRecaudado = total
                }
            });
        }

        [HttpPost("reservar")]
        public IActionResult Reservar([FromBody] ReservaRequest request)
        {
            var ok = _reservaService.Reservar(
                request.ViajeId,
                request.PasajeroId,
                request.NumeroAsiento,
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

        [HttpPost("cancelar-reserva")]
        public IActionResult CancelarReserva([FromBody] CancelarReservaRequest request)
        {
            var ok = _reservaService.CancelarReserva(request.ReservaId, out string mensaje);

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
    }
}