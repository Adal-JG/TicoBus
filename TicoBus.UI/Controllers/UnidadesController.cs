using Microsoft.AspNetCore.Mvc;
using TicoBus.Model;
using TicoBus.UI.ApiClients;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class UnidadesController : BaseController
    {
        private readonly UnidadesApiClient _unidadesApiClient;

        public UnidadesController(UnidadesApiClient unidadesApiClient)
        {
            _unidadesApiClient = unidadesApiClient;
        }

        public async Task<IActionResult> Index(string? filtro)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            ViewBag.Filtro = filtro;

            var unidades = await _unidadesApiClient.Listar(filtro);

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
        public async Task<IActionResult> Create(UnidadViewModel model)
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

            var response = await _unidadesApiClient.Agregar(unidad);

            if (response == null || !response.Exitoso)
            {
                ModelState.AddModelError("", response?.Mensaje ?? "No se pudo registrar la unidad.");
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

            var unidad = await _unidadesApiClient.ObtenerPorId(id);

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
        public async Task<IActionResult> Edit(UnidadViewModel model)
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

            var response = await _unidadesApiClient.Actualizar(model.Id, unidad);

            if (response == null || !response.Exitoso)
            {
                ModelState.AddModelError("", response?.Mensaje ?? "No se pudo actualizar la unidad.");
                return View(model);
            }

            TempData["Exito"] = response.Mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            TempData["Error"] = "La eliminación de unidades no está habilitada en esta segunda entrega.";
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