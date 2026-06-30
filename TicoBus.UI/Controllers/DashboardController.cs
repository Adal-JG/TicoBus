using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.DA.Repositories;
using TicoBus.Model;

namespace TicoBus.UI.Controllers
{
    public class DashboardController : BaseController
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

        public IActionResult Index()
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var rol = HttpContext.Session.GetString("Rol");
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.Rol = rol;

            if (rol == "Chofer" && usuarioId != null)
            {
                var chofer = _choferService.ObtenerPorUsuarioId(usuarioId.Value);

                if (chofer != null)
                {
                    var viajesHoy = _viajeService.ListarPorChoferHoy(chofer.Id);
                    var proximoViaje = _viajeService.ObtenerProximoViajeChofer(chofer.Id);

                    ViewBag.ViajesHoy = viajesHoy;
                    ViewBag.ProximoViaje = proximoViaje;
                    ViewBag.TotalViajesHoy = viajesHoy.Count;
                    ViewBag.PasajerosRegistrados = viajesHoy.Sum(v => v.Reservas?.Count(r => r.Activa) ?? 0);
                    ViewBag.AsientosDisponibles = viajesHoy.Sum(v =>
                        (v.Unidad?.CapacidadPasajeros ?? 0) - (v.Reservas?.Count(r => r.Activa) ?? 0));
                }

                return View("Chofer");
            }

            ViewBag.TotalChoferes = _dashboardRepository.TotalChoferes();
            ViewBag.TotalPasajeros = _dashboardRepository.TotalPasajeros();
            ViewBag.TotalRutas = _dashboardRepository.TotalRutas();
            ViewBag.TotalUnidades = _dashboardRepository.TotalUnidades();
            ViewBag.TotalViajesProgramados = _dashboardRepository.TotalViajesProgramados();
            ViewBag.TotalViajesEnCurso = _dashboardRepository.TotalViajesEnCurso();
            ViewBag.TotalViajesCancelados = _dashboardRepository.TotalViajesCancelados();

            return View();
        }
    }
}