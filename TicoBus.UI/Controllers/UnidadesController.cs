using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class UnidadesController : BaseController
    {
        private readonly IUnidadService _unidadService;

        public UnidadesController(IUnidadService unidadService)
        {
            _unidadService = unidadService;
        }

        public IActionResult Index(string? filtro)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            ViewBag.Filtro = filtro;
            var unidades = _unidadService.Listar(filtro);

            return View(unidades);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            return View(new UnidadViewModel());
        }

        [HttpPost]
        public IActionResult Create(UnidadViewModel model)
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

            var unidad = ConvertirAEntidad(model);

            var resultado = _unidadService.Agregar(unidad, out string mensaje);

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

            var unidad = _unidadService.ObtenerPorId(id);

            if (unidad == null)
            {
                TempData["Error"] = "Unidad no encontrada.";
                return RedirectToAction("Index");
            }

            var model = new UnidadViewModel
            {
                Id = unidad.Id,
                Placa = unidad.Placa,
                Modelo = unidad.Modelo,
                AnioFabricacion = unidad.AnioFabricacion,
                CapacidadPasajeros = unidad.CapacidadPasajeros
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UnidadViewModel model)
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

            var unidad = ConvertirAEntidad(model);

            var resultado = _unidadService.Actualizar(unidad, out string mensaje);

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

            var resultado = _unidadService.Eliminar(id, out string mensaje);

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

        private Unidad ConvertirAEntidad(UnidadViewModel model)
        {
            return new Unidad
            {
                Id = model.Id,
                Placa = model.Placa,
                Modelo = model.Modelo,
                AnioFabricacion = model.AnioFabricacion,
                CapacidadPasajeros = model.CapacidadPasajeros
            };
        }
    }
}