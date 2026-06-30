using Microsoft.AspNetCore.Mvc;
using TicoBus.UI.ApiClients;

namespace TicoBus.UI.Controllers
{
    public class ViajesCanceladosController : BaseController
    {
        private readonly ViajesCanceladosApiClient _viajesCanceladosApiClient;

        public ViajesCanceladosController(ViajesCanceladosApiClient viajesCanceladosApiClient)
        {
            _viajesCanceladosApiClient = viajesCanceladosApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var validacion = ValidarRol("Administrador");

            if (validacion != null)
            {
                return validacion;
            }

            var viajes = await _viajesCanceladosApiClient.Listar();

            return View(viajes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var validacion = ValidarRol("Administrador");

            if (validacion != null)
            {
                return validacion;
            }

            var viaje = await _viajesCanceladosApiClient.ObtenerPorId(id);

            if (viaje == null)
            {
                TempData["Error"] = "Viaje cancelado no encontrado.";
                return RedirectToAction("Index");
            }

            return View(viaje);
        }
    }
}