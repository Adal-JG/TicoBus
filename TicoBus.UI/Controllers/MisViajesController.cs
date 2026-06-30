using Microsoft.AspNetCore.Mvc;
using TicoBus.Model;
using TicoBus.UI.ApiClients;

namespace TicoBus.UI.Controllers
{
    public class MisViajesController : BaseController
    {
        private readonly MisViajesApiClient _misViajesApiClient;

        public MisViajesController(MisViajesApiClient misViajesApiClient)
        {
            _misViajesApiClient = misViajesApiClient;
        }

        public async Task<IActionResult> Index()
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

            var reservas = await _misViajesApiClient.ListarPorUsuario(usuarioId.Value);

            return View(reservas);
        }

        public async Task<IActionResult> Detalle(int id)
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

            var reservas = await _misViajesApiClient.ListarPorUsuario(usuarioId.Value);
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