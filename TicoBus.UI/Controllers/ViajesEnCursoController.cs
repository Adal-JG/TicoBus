using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicoBus.BL.Interfaces;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class ViajesEnCursoController : BaseController
    {
        private readonly IViajeService _viajeService;
        private readonly IReservaService _reservaService;
        private readonly IPasajeroService _pasajeroService;

        public ViajesEnCursoController(
            IViajeService viajeService,
            IReservaService reservaService,
            IPasajeroService pasajeroService)
        {
            _viajeService = viajeService;
            _reservaService = reservaService;
            _pasajeroService = pasajeroService;
        }

        public IActionResult Index()
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var viajes = _viajeService.ListarEnCurso();

            return View(viajes);
        }

        public IActionResult Administrar(int id)
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

            var reservas = _reservaService.ListarPorViaje(id);

            ViewBag.Reservas = reservas;
            ViewBag.AsientosOcupados = reservas.Select(r => r.NumeroAsiento).ToList();

            ViewBag.Pasajeros = new SelectList(
                _pasajeroService.Listar(null).Select(p => new
                {
                    p.Id,
                    NombreCompleto = $"{p.Nombre} {p.Apellidos} - {p.Identificacion}"
                }),
                "Id",
                "NombreCompleto");

            ViewBag.Ocupados = _reservaService.CantidadReservasActivas(id);
            ViewBag.Total = _reservaService.TotalRecaudado(id);
            ViewBag.Capacidad = viaje.Unidad?.CapacidadPasajeros ?? 0;

            return View(new ReservaViewModel
            {
                ViajeId = id
            });
        }

        [HttpPost]
        public IActionResult Reservar(ReservaViewModel model)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var resultado = _reservaService.Reservar(
                model.ViajeId,
                model.PasajeroId,
                model.NumeroAsiento,
                out string mensaje);

            TempData[resultado ? "Exito" : "Error"] = mensaje;

            return RedirectToAction("Administrar", new { id = model.ViajeId });
        }

        [HttpPost]
        public IActionResult CancelarReserva(int reservaId, int viajeId)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var resultado = _reservaService.CancelarReserva(reservaId, out string mensaje);
            TempData[resultado ? "Exito" : "Error"] = mensaje;

            return RedirectToAction("Administrar", new { id = viajeId });
        }

        [HttpGet]
        public IActionResult Finalizar(int id)
        {
            var viaje = _viajeService.ObtenerPorId(id);

            if (viaje == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Reservas = _reservaService.ListarPorViaje(id);
            ViewBag.Ocupados = _reservaService.CantidadReservasActivas(id);
            ViewBag.Total = _reservaService.TotalRecaudado(id);

            return View("Finalizar", viaje);
        }

        [HttpPost]
        public IActionResult ConfirmarFinalizar(int id)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var resultado = _viajeService.Finalizar(id, out string mensaje);
            TempData[resultado ? "Exito" : "Error"] = mensaje;

            return RedirectToAction("Index");
        }
    }
}