using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.SI.DTOs;

namespace TicoBus.SI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MisViajesController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public MisViajesController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpGet("{pasajeroId}")]
        public IActionResult Obtener(int pasajeroId)
        {
            return Ok(new ApiResponse<List<Reserva>>
            {
                Exitoso = true,
                Datos = _reservaService.ListarPorPasajero(pasajeroId)
            });
        }
    }
}