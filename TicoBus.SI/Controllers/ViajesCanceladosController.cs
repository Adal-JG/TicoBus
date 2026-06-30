using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.SI.DTOs;

namespace TicoBus.SI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViajesCanceladosController : ControllerBase
    {
        private readonly IViajeService _viajeService;

        public ViajesCanceladosController(IViajeService viajeService)
        {
            _viajeService = viajeService;
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(new ApiResponse<List<Viaje>>
            {
                Exitoso = true,
                Datos = _viajeService.ListarCancelados()
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
    }
}