using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicoBus.UI.ApiClients;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class ViajesEnCursoController : BaseController
    {
        private readonly ViajesEnCursoApiClient _viajesEnCursoApiClient;
        private readonly PasajerosApiClient _pasajerosApiClient;

        public ViajesEnCursoController(
            ViajesEnCursoApiClient viajesEnCursoApiClient,
            PasajerosApiClient pasajerosApiClient)
        {
            _viajesEnCursoApiClient = viajesEnCursoApiClient;
            _pasajerosApiClient = pasajerosApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var viajes = await _viajesEnCursoApiClient.Listar();

            return View(viajes);
        }

        public async Task<IActionResult> Administrar(int id)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var viaje = await _viajesEnCursoApiClient.ObtenerPorId(id);

            if (viaje == null)
            {
                TempData["Error"] = "Viaje no encontrado.";
                return RedirectToAction("Index");
            }

            var reservas = await _viajesEnCursoApiClient.ListarReservas(id);
            var pasajeros = await _pasajerosApiClient.Listar(null);

            ViewBag.Reservas = reservas;
            ViewBag.AsientosOcupados = reservas.Select(r => r.NumeroAsiento).ToList();

            ViewBag.Pasajeros = new SelectList(
                pasajeros.Select(p => new
                {
                    p.Id,
                    NombreCompleto = $"{p.Nombre} {p.Apellidos} - {p.Identificacion}"
                }),
                "Id",
                "NombreCompleto");

            ViewBag.Ocupados = reservas.Count(r => r.Activa);
            ViewBag.Total = reservas.Where(r => r.Activa).Sum(r => r.MontoPagado);
            ViewBag.Capacidad = viaje.Unidad?.CapacidadPasajeros ?? 0;

            return View(new ReservaViewModel
            {
                ViajeId = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Reservar(ReservaViewModel model)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var response = await _viajesEnCursoApiClient.Reservar(
                model.ViajeId,
                model.PasajeroId,
                model.NumeroAsiento);

            TempData[response != null && response.Exitoso ? "Exito" : "Error"] =
                response?.Mensaje ?? "No se pudo registrar la reserva.";

            return RedirectToAction("Administrar", new { id = model.ViajeId });
        }

        [HttpPost]
        public async Task<IActionResult> CancelarReserva(int reservaId, int viajeId)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var response = await _viajesEnCursoApiClient.CancelarReserva(reservaId);

            TempData[response != null && response.Exitoso ? "Exito" : "Error"] =
                response?.Mensaje ?? "No se pudo cancelar la reserva.";

            return RedirectToAction("Administrar", new { id = viajeId });
        }

        [HttpGet]
        public async Task<IActionResult> Finalizar(int id)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var viaje = await _viajesEnCursoApiClient.ObtenerPorId(id);

            if (viaje == null)
            {
                TempData["Error"] = "Viaje no encontrado.";
                return RedirectToAction("Index");
            }

            var reservas = await _viajesEnCursoApiClient.ListarReservas(id);

            ViewBag.Reservas = reservas;
            ViewBag.Ocupados = reservas.Count(r => r.Activa);
            ViewBag.Total = reservas.Where(r => r.Activa).Sum(r => r.MontoPagado);

            return View("Finalizar", viaje);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarFinalizar(int id)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var response = await _viajesEnCursoApiClient.Finalizar(id);

            TempData[response != null && response.Exitoso ? "Exito" : "Error"] =
                response?.Mensaje ?? "No se pudo finalizar el viaje.";

            return RedirectToAction("Index");
        }
    }
}