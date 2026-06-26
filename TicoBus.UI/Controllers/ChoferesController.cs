using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class ChoferesController : BaseController
    {
        private readonly IChoferService _choferService;

        public ChoferesController(IChoferService choferService)
        {
            _choferService = choferService;
        }

        public IActionResult Index(string? filtro)
        {
            var validacion = ValidarRol("Administrador");

            if (validacion != null)
            {
                return validacion;
            }

            ViewBag.Filtro = filtro;
            var choferes = _choferService.Listar(filtro);

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
        public IActionResult Create(ChoferViewModel model)
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

            var resultado = _choferService.Agregar(chofer, out string mensaje);

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
            var validacion = ValidarRol("Administrador");

            if (validacion != null)
            {
                return validacion;
            }

            var chofer = _choferService.ObtenerPorId(id);

            if (chofer == null)
            {
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
        public IActionResult Edit(ChoferViewModel model)
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

            var resultado = _choferService.Actualizar(chofer, out string mensaje);

            if (!resultado)
            {
                ViewBag.Error = mensaje;
                return View(model);
            }

            TempData["Exito"] = mensaje;
            return RedirectToAction("Index");
        }
    }
}