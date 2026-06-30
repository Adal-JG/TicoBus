using Microsoft.AspNetCore.Mvc;
using TicoBus.UI.ApiClients;

namespace TicoBus.UI.Controllers
{
    public class MisViajesController : BaseController
    {
        private readonly MisViajesApiClient _misViajesApiClient;
        private readonly PasajerosApiClient _pasajerosApiClient;

        public MisViajesController(
            MisViajesApiClient misViajesApiClient,
            PasajerosApiClient pasajerosApiClient)
        {
            _misViajesApiClient = misViajesApiClient;
            _pasajerosApiClient = pasajerosApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var validacion = ValidarRol("Pasajero");
            if (validacion != null) return validacion;

            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null) return RedirectToAction("Index", "Login");

            var pasajeros = await _pasajerosApiClient.Listar(null);
            var pasajero = pasajeros.FirstOrDefault(p => p.UsuarioId == usuarioId.Value);

            if (pasajero == null)
            {
                TempData["Error"] = "No se encontró el pasajero asociado al usuario.";
                return View(new List<TicoBus.Model.Reserva>());
            }

            var reservas = await _misViajesApiClient.ListarPorUsuario(pasajero.Id);

            return View(reservas);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var validacion = ValidarRol("Pasajero");
            if (validacion != null) return validacion;

            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null) return RedirectToAction("Index", "Login");

            var pasajeros = await _pasajerosApiClient.Listar(null);
            var pasajero = pasajeros.FirstOrDefault(p => p.UsuarioId == usuarioId.Value);

            if (pasajero == null)
            {
                TempData["Error"] = "No se encontró el pasajero asociado al usuario.";
                return RedirectToAction("Index");
            }

            var reservas = await _misViajesApiClient.ListarPorUsuario(pasajero.Id);
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