using Microsoft.AspNetCore.Mvc;
using TicoBus.DA.Repositories;

namespace TicoBus.UI.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly DashboardRepository _dashboardRepository;

        public DashboardController(DashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public IActionResult Index()
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.Rol = HttpContext.Session.GetString("Rol");

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