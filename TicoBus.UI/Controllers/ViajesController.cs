using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class ViajesController : BaseController
    {
        private readonly IViajeService _viajeService;
        private readonly IRutaService _rutaService;
        private readonly IUnidadService _unidadService;
        private readonly IChoferService _choferService;

        public ViajesController(
            IViajeService viajeService,
            IRutaService rutaService,
            IUnidadService unidadService,
            IChoferService choferService)
        {
            _viajeService = viajeService;
            _rutaService = rutaService;
            _unidadService = unidadService;
            _choferService = choferService;
        }

        public IActionResult Index(string? filtro)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            ViewBag.Filtro = filtro;
            var viajes = _viajeService.Listar(filtro);

            return View(viajes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            CargarListas();
            return View(new ViajeViewModel
            {
                FechaHoraSalida = DateTime.Now,
                FechaHoraLlegadaEstimada = DateTime.Now.AddHours(2)
            });
        }

        [HttpPost]
        public IActionResult Create(ViajeViewModel model)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            if (!ModelState.IsValid)
            {
                CargarListas();
                return View(model);
            }

            var viaje = ConvertirAEntidad(model);
            var resultado = _viajeService.Agregar(viaje, out string mensaje);

            if (!resultado)
            {
                ModelState.AddModelError("", mensaje);
                CargarListas();
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

            var viaje = _viajeService.ObtenerPorId(id);

            if (viaje == null)
            {
                TempData["Error"] = "Viaje no encontrado.";
                return RedirectToAction("Index");
            }

            if (viaje.Estado != EstadoViaje.Programado)
            {
                TempData["Error"] = "Solo se pueden editar viajes en estado Programado.";
                return RedirectToAction("Index");
            }

            var model = new ViajeViewModel
            {
                Id = viaje.Id,
                RutaId = viaje.RutaId,
                UnidadId = viaje.UnidadId,
                ChoferId = viaje.ChoferId,
                FechaHoraSalida = viaje.FechaHoraSalida,
                FechaHoraLlegadaEstimada = viaje.FechaHoraLlegadaEstimada
            };

            CargarListas();
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ViajeViewModel model)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            if (!ModelState.IsValid)
            {
                CargarListas();
                return View(model);
            }

            var viaje = ConvertirAEntidad(model);
            var resultado = _viajeService.Actualizar(viaje, out string mensaje);

            if (!resultado)
            {
                ModelState.AddModelError("", mensaje);
                CargarListas();
                return View(model);
            }

            TempData["Exito"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Iniciar(int id)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var resultado = _viajeService.Iniciar(id, out string mensaje);
            TempData[resultado ? "Exito" : "Error"] = mensaje;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Cancelar(int id)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var viaje = _viajeService.ObtenerPorId(id);

            if (viaje == null)
            {
                TempData["Error"] = "Viaje no encontrado.";
                return RedirectToAction("Index");
            }

            if (viaje.Estado != EstadoViaje.Programado)
            {
                TempData["Error"] = "Solo se pueden cancelar viajes en estado Programado.";
                return RedirectToAction("Index");
            }

            var model = new ViajeViewModel
            {
                Id = viaje.Id,
                RutaId = viaje.RutaId,
                UnidadId = viaje.UnidadId,
                ChoferId = viaje.ChoferId,
                FechaHoraSalida = viaje.FechaHoraSalida,
                FechaHoraLlegadaEstimada = viaje.FechaHoraLlegadaEstimada
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Cancelar(ViajeViewModel model)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var resultado = _viajeService.Cancelar(model.Id, model.MotivoCancelacion ?? "", out string mensaje);

            if (!resultado)
            {
                ModelState.AddModelError("", mensaje);
                return View(model);
            }

            TempData["Exito"] = mensaje;
            return RedirectToAction("Index");
        }

        private Viaje ConvertirAEntidad(ViajeViewModel model)
        {
            return new Viaje
            {
                Id = model.Id,
                RutaId = model.RutaId,
                UnidadId = model.UnidadId,
                ChoferId = model.ChoferId,
                FechaHoraSalida = model.FechaHoraSalida,
                FechaHoraLlegadaEstimada = model.FechaHoraLlegadaEstimada
            };
        }

        private void CargarListas()
        {
            var rutas = _rutaService.Listar(null);

            ViewBag.Rutas = new SelectList(
            rutas.Select(r => new
            {
                r.Id,
                Texto = $"{r.Origen} → {r.Destino} ({(int)r.DuracionEstimada.TotalHours:00}:{r.DuracionEstimada.Minutes:00})"
            }),
            "Id",
            "Texto");

            ViewBag.DuracionesRutas = rutas.Select(r => new
            {
                id = r.Id,
                minutos = (int)r.DuracionEstimada.TotalMinutes
            }).ToList();

            ViewBag.Unidades = new SelectList(_unidadService.Listar(null), "Id", "Placa");

            ViewBag.Choferes = new SelectList(
                _choferService.Listar(null).Select(c => new
                {
                    c.Id,
                    NombreCompleto = $"{c.Nombre} {c.Apellidos}"
                }),
                "Id",
                "NombreCompleto");
        }
    }
}