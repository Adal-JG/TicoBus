using Microsoft.AspNetCore.Mvc;
using TicoBus.Model;
using TicoBus.UI.ApiClients;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class PasajerosController : BaseController
    {
        private readonly PasajerosApiClient _pasajerosApiClient;

        public PasajerosController(PasajerosApiClient pasajerosApiClient)
        {
            _pasajerosApiClient = pasajerosApiClient;
        }

        public async Task<IActionResult> Index(string? filtro)
        {
            var validacion = ValidarRol("Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            ViewBag.Filtro = filtro;

            var pasajeros = await _pasajerosApiClient.Listar(filtro);

            return View(pasajeros);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var validacion = ValidarRol("Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            return View(new PasajeroViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(PasajeroViewModel model)
        {
            var validacion = ValidarRol("Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var pasajero = new Pasajero
            {
                Identificacion = model.Identificacion,
                Nombre = model.Nombre,
                Apellidos = model.Apellidos,
                Correo = model.Correo
            };

            var response = await _pasajerosApiClient.Agregar(pasajero);

            if (response == null || !response.Exitoso)
            {
                ViewBag.Error = response?.Mensaje ?? "No se pudo registrar el pasajero.";
                return View(model);
            }

            TempData["Exito"] = response.Mensaje;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var validacion = ValidarRol("Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var pasajero = await _pasajerosApiClient.ObtenerPorId(id);

            if (pasajero == null)
            {
                TempData["Error"] = "Pasajero no encontrado.";
                return RedirectToAction("Index");
            }

            var model = new PasajeroViewModel
            {
                Id = pasajero.Id,
                Identificacion = pasajero.Identificacion,
                Nombre = pasajero.Nombre,
                Apellidos = pasajero.Apellidos,
                Correo = pasajero.Correo
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PasajeroViewModel model)
        {
            var validacion = ValidarRol("Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var pasajero = new Pasajero
            {
                Id = model.Id,
                Identificacion = model.Identificacion,
                Nombre = model.Nombre,
                Apellidos = model.Apellidos,
                Correo = model.Correo
            };

            var response = await _pasajerosApiClient.Actualizar(model.Id, pasajero);

            if (response == null || !response.Exitoso)
            {
                ViewBag.Error = response?.Mensaje ?? "No se pudo actualizar el pasajero.";
                return View(model);
            }

            TempData["Exito"] = response.Mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            TempData["Error"] = "La eliminación de pasajeros no está habilitada en esta segunda entrega.";
            return RedirectToAction("Index");
        }
    }
}