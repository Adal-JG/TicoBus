using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.DA.Repositories;
using TicoBus.Model;
using TicoBus.SI.DTOs;

namespace TicoBus.SI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardRepository _dashboardRepository;
        private readonly IChoferService _choferService;
        private readonly IViajeService _viajeService;

        public DashboardController(
            DashboardRepository dashboardRepository,
            IChoferService choferService,
            IViajeService viajeService)
        {
            _dashboardRepository = dashboardRepository;
            _choferService = choferService;
            _viajeService = viajeService;
        }

        [HttpGet("admin")]
        public IActionResult Admin()
        {
            return Ok(new ApiResponse<object>
            {
                Exitoso = true,
                Datos = new
                {
                    totalChoferes = _dashboardRepository.TotalChoferes(),
                    totalPasajeros = _dashboardRepository.TotalPasajeros(),
                    totalRutas = _dashboardRepository.TotalRutas(),
                    totalUnidades = _dashboardRepository.TotalUnidades(),
                    totalViajesProgramados = _dashboardRepository.TotalViajesProgramados(),
                    totalViajesEnCurso = _dashboardRepository.TotalViajesEnCurso(),
                    totalViajesCancelados = _dashboardRepository.TotalViajesCancelados()
                }
            });
        }

        [HttpGet("chofer/{usuarioId}")]
        public IActionResult Chofer(int usuarioId)
        {
            var chofer = _choferService.ObtenerPorUsuarioId(usuarioId);

            if (chofer == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Exitoso = false,
                    Mensaje = "Chofer no encontrado."
                });
            }

            var viajesHoy = _viajeService.ListarPorChoferHoy(chofer.Id);
            var proximoViaje = _viajeService.ObtenerProximoViajeChofer(chofer.Id);

            return Ok(new ApiResponse<object>
            {
                Exitoso = true,
                Datos = new
                {
                    viajesHoy,
                    proximoViaje,
                    totalViajesHoy = viajesHoy.Count,
                    pasajerosRegistrados = viajesHoy.Sum(v => v.Reservas?.Count(r => r.Activa) ?? 0),
                    asientosDisponibles = viajesHoy.Sum(v =>
                        (v.Unidad?.CapacidadPasajeros ?? 0) - (v.Reservas?.Count(r => r.Activa) ?? 0))
                }
            });
        }
    }
}