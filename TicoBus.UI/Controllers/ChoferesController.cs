using Microsoft.AspNetCore.Mvc;
using TicoBus.Model;
using TicoBus.UI.ApiClients;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class ChoferesController : BaseController
    {
        private readonly ChoferesApiClient _choferesApiClient;

        public ChoferesController(ChoferesApiClient choferesApiClient)
        {
            _choferesApiClient = choferesApiClient;
        }

        public async Task<IActionResult> Index(string? filtro)
        {
            var validacion = ValidarRol("Administrador");

            if (validacion != null)
            {
                return validacion;
            }

            ViewBag.Filtro = filtro;

            var choferes = await _choferesApiClient.Listar(filtro);

            return View(choferes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var validacion = ValidarRol("Administrador");

            if (validacion != null)
            {
                return validacion;
            }

            return View(new ChoferViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChoferViewModel model)
        {
            var validacion = ValidarRol("Administrador");

            if (validacion != null)
            {
                return validacion;
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var chofer = new Chofer
            {
                Identificacion = model.Identificacion,
                Nombre = model.Nombre,
                Apellidos = model.Apellidos,
                Correo = model.Correo
            };

            var response = await _choferesApiClient.Agregar(chofer);

            if (response == null || !response.Exitoso)
            {
                ViewBag.Error = response?.Mensaje ?? "No se pudo registrar el chofer.";
                return View(model);
            }

            TempData["Exito"] = response.Mensaje;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var validacion = ValidarRol("Administrador");

            if (validacion != null)
            {
                return validacion;
            }

            var chofer = await _choferesApiClient.ObtenerPorId(id);

            if (chofer == null)
            {
                TempData["Error"] = "Chofer no encontrado.";
                return RedirectToAction("Index");
            }

            var model = new ChoferViewModel
            {
                Id = chofer.Id,
                Identificacion = chofer.Identificacion,
                Nombre = chofer.Nombre,
                Apellidos = chofer.Apellidos,
                Correo = chofer.Correo
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChoferViewModel model)
        {
            var validacion = ValidarRol("Administrador");

            if (validacion != null)
            {
                return validacion;
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var chofer = new Chofer
            {
                Id = model.Id,
                Identificacion = model.Identificacion,
                Nombre = model.Nombre,
                Apellidos = model.Apellidos,
                Correo = model.Correo
            };

            var response = await _choferesApiClient.Actualizar(model.Id, chofer);

            if (response == null || !response.Exitoso)
            {
                ViewBag.Error = response?.Mensaje ?? "No se pudo actualizar el chofer.";
                return View(model);
            }

            TempData["Exito"] = response.Mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            TempData["Error"] = "La eliminación de choferes no está habilitada en esta segunda entrega.";
            return RedirectToAction("Index");
        }
    }
}