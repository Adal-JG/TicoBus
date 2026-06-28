using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class RutasController : BaseController
    {
        private readonly IRutaService _rutaService;

        public RutasController(IRutaService rutaService)
        {
            _rutaService = rutaService;
        }

        public IActionResult Index(string? filtro)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            ViewBag.Filtro = filtro;
            var rutas = _rutaService.Listar(filtro);

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
        public IActionResult Create(RutaViewModel model)
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

            var resultado = _rutaService.Agregar(ruta, out string mensaje);

            if (!resultado)
            {
                ModelState.AddModelError("", mensaje);
                return View(model);
            }

            TempData["Exito"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var ruta = _rutaService.ObtenerPorId(id);

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
        public IActionResult Edit(RutaViewModel model)
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

            var resultado = _rutaService.Actualizar(ruta, out string mensaje);

            if (!resultado)
            {
                ModelState.AddModelError("", mensaje);
                return View(model);
            }

            TempData["Exito"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var resultado = _rutaService.Eliminar(id, out string mensaje);

            if (resultado)
            {
                TempData["Exito"] = mensaje;
            }
            else
            {
                TempData["Error"] = mensaje;
            }

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