using Microsoft.AspNetCore.Mvc;
using TicoBus.Model;
using TicoBus.UI.ApiClients;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class RutasController : BaseController
    {
        private readonly RutasApiClient _rutasApiClient;

        public RutasController(RutasApiClient rutasApiClient)
        {
            _rutasApiClient = rutasApiClient;
        }

        public async Task<IActionResult> Index(string? filtro)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            ViewBag.Filtro = filtro;

            var rutas = await _rutasApiClient.Listar(filtro);

            return View(rutas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            return View(new RutaViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RutaViewModel model)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ruta = ConvertirAEntidad(model);

            var response = await _rutasApiClient.Agregar(ruta);

            if (response == null || !response.Exitoso)
            {
                ModelState.AddModelError("", response?.Mensaje ?? "No se pudo registrar la ruta.");
                return View(model);
            }

            TempData["Exito"] = response.Mensaje;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var ruta = await _rutasApiClient.ObtenerPorId(id);

            if (ruta == null)
            {
                TempData["Error"] = "Ruta no encontrada.";
                return RedirectToAction("Index");
            }

            var model = new RutaViewModel
            {
                Id = ruta.Id,
                Nombre = ruta.Nombre,
                Origen = ruta.Origen,
                Destino = ruta.Destino,
                DuracionEstimada = $"{(int)ruta.DuracionEstimada.TotalHours:00}:{ruta.DuracionEstimada.Minutes:00}",
                PrecioBase = ruta.PrecioBase
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RutaViewModel model)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ruta = ConvertirAEntidad(model);

            var response = await _rutasApiClient.Actualizar(model.Id, ruta);

            if (response == null || !response.Exitoso)
            {
                ModelState.AddModelError("", response?.Mensaje ?? "No se pudo actualizar la ruta.");
                return View(model);
            }

            TempData["Exito"] = response.Mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            TempData["Error"] = "La eliminación de rutas no está habilitada en esta segunda entrega.";
            return RedirectToAction("Index");
        }

        private Ruta ConvertirAEntidad(RutaViewModel model)
        {
            return new Ruta
            {
                Id = model.Id,
                Nombre = model.Nombre,
                Origen = model.Origen,
                Destino = model.Destino,
                DuracionEstimada = TimeSpan.Parse(model.DuracionEstimada),
                PrecioBase = model.PrecioBase
            };
        }
    }
}