using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;

namespace TicoBus.UI.Controllers
{
    public class MisViajesController : BaseController
    {
        private readonly IPasajeroService _pasajeroService;
        private readonly IReservaService _reservaService;

        public MisViajesController(
            IPasajeroService pasajeroService,
            IReservaService reservaService)
        {
            _pasajeroService = pasajeroService;
            _reservaService = reservaService;
        }

        public IActionResult Index()
        {
            var validacion = ValidarRol("Pasajero");

            if (validacion != null)
            {
                return validacion;
            }

            var usuarioId = ObtenerUsuarioId();

            if (usuarioId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var pasajero = _pasajeroService.ObtenerPorUsuarioId(usuarioId.Value);

            if (pasajero == null)
            {
                TempData["Error"] = "No se encontró el pasajero asociado al usuario.";
                return View(new List<TicoBus.Model.Reserva>());
            }

            var reservas = _reservaService.ListarPorPasajero(pasajero.Id);

            return View(reservas);
        }

        public IActionResult Detalle(int id)
        {
            var validacion = ValidarRol("Pasajero");

            if (validacion != null)
            {
                return validacion;
            }

            var usuarioId = ObtenerUsuarioId();

            if (usuarioId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var pasajero = _pasajeroService.ObtenerPorUsuarioId(usuarioId.Value);

            if (pasajero == null)
            {
                TempData["Error"] = "No se encontró el pasajero asociado al usuario.";
                return RedirectToAction("Index");
            }

            var reservas = _reservaService.ListarPorPasajero(pasajero.Id);
            var reserva = reservas.FirstOrDefault(r => r.Id == id);

            if (reserva == null)
            {
                TempData["Error"] = "Reserva no encontrada.";
                return RedirectToAction("Index");
            }

            return View(reserva);
        }
    }
}