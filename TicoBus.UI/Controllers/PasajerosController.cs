using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class PasajerosController : BaseController
    {
        private readonly IPasajeroService _pasajeroService;

        public PasajerosController(IPasajeroService pasajeroService)
        {
            _pasajeroService = pasajeroService;
        }

        public IActionResult Index(string? filtro)
        {
            var validacion = ValidarRol("Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            ViewBag.Filtro = filtro;
            var pasajeros = _pasajeroService.Listar(filtro);

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
        public IActionResult Create(PasajeroViewModel model)
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

            var resultado = _pasajeroService.Agregar(pasajero, out string mensaje);

            if (!resultado)
            {
                ViewBag.Error = mensaje;
                return View(model);
            }

            TempData["Exito"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var validacion = ValidarRol("Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var pasajero = _pasajeroService.ObtenerPorId(id);

            if (pasajero == null)
            {
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
        public IActionResult Edit(PasajeroViewModel model)
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

            var resultado = _pasajeroService.Actualizar(pasajero, out string mensaje);

            if (!resultado)
            {
                ViewBag.Error = mensaje;
                return View(model);
            }

            TempData["Exito"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var validacion = ValidarRol("Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var resultado = _pasajeroService.Eliminar(id, out string mensaje);

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
    }
}