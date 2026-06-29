using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;

namespace TicoBus.UI.Controllers
{
    public class ViajesCanceladosController : BaseController
    {
        private readonly IViajeService _viajeService;

        public ViajesCanceladosController(IViajeService viajeService)
        {
            _viajeService = viajeService;
        }

        public IActionResult Index()
        {
            var validacion = ValidarRol("Administrador");

            if (validacion != null)
            {
                return validacion;
            }

            var viajes = _viajeService.ListarCancelados();

            return View(viajes);
        }

        public IActionResult Details(int id)
        {
            var validacion = ValidarRol("Administrador");

            if (validacion != null)
            {
                return validacion;
            }

            var viaje = _viajeService.ObtenerPorId(id);

            if (viaje == null)
            {
                TempData["Error"] = "Viaje cancelado no encontrado.";
                return RedirectToAction("Index");
            }

            return View(viaje);
        }
    }
}