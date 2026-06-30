using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TicoBus.UI.ApiClients;

namespace TicoBus.UI.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly DashboardApiClient _dashboardApiClient;

        public DashboardController(DashboardApiClient dashboardApiClient)
        {
            _dashboardApiClient = dashboardApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var rol = HttpContext.Session.GetString("Rol");
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.Rol = rol;

            if (rol == "Chofer")
            {
                ViewBag.ViajesHoyJson = "[]";
                ViewBag.ProximoViajeJson = "null";
                ViewBag.TotalViajesHoy = 0;
                ViewBag.PasajerosRegistrados = 0;
                ViewBag.AsientosDisponibles = 0;

                if (usuarioId != null)
                {
                    try
                    {
                        var datos = await _dashboardApiClient.ObtenerChofer(usuarioId.Value);

                        if (datos != null)
                        {
                            var json = datos.Value;

                            if (json.TryGetProperty("viajesHoy", out var viajesHoy))
                                ViewBag.ViajesHoyJson = viajesHoy.GetRawText();

                            if (json.TryGetProperty("proximoViaje", out var proximoViaje))
                                ViewBag.ProximoViajeJson = proximoViaje.GetRawText();

                            if (json.TryGetProperty("totalViajesHoy", out var totalViajesHoy))
                                ViewBag.TotalViajesHoy = totalViajesHoy.GetInt32();

                            if (json.TryGetProperty("pasajerosRegistrados", out var pasajerosRegistrados))
                                ViewBag.PasajerosRegistrados = pasajerosRegistrados.GetInt32();

                            if (json.TryGetProperty("asientosDisponibles", out var asientosDisponibles))
                                ViewBag.AsientosDisponibles = asientosDisponibles.GetInt32();
                        }
                    }
                    catch
                    {
                        TempData["Error"] = "No se pudo cargar la información del dashboard del chofer.";
                    }
                }

                return View("Chofer");
            }

            try
            {
                var admin = await _dashboardApiClient.ObtenerAdmin();

                ViewBag.TotalChoferes = 0;
                ViewBag.TotalPasajeros = 0;
                ViewBag.TotalRutas = 0;
                ViewBag.TotalUnidades = 0;
                ViewBag.TotalViajesProgramados = 0;
                ViewBag.TotalViajesEnCurso = 0;
                ViewBag.TotalViajesCancelados = 0;

                if (admin != null)
                {
                    var json = admin.Value;

                    if (json.TryGetProperty("totalChoferes", out var totalChoferes))
                        ViewBag.TotalChoferes = totalChoferes.GetInt32();

                    if (json.TryGetProperty("totalPasajeros", out var totalPasajeros))
                        ViewBag.TotalPasajeros = totalPasajeros.GetInt32();

                    if (json.TryGetProperty("totalRutas", out var totalRutas))
                        ViewBag.TotalRutas = totalRutas.GetInt32();

                    if (json.TryGetProperty("totalUnidades", out var totalUnidades))
                        ViewBag.TotalUnidades = totalUnidades.GetInt32();

                    if (json.TryGetProperty("totalViajesProgramados", out var totalViajesProgramados))
                        ViewBag.TotalViajesProgramados = totalViajesProgramados.GetInt32();

                    if (json.TryGetProperty("totalViajesEnCurso", out var totalViajesEnCurso))
                        ViewBag.TotalViajesEnCurso = totalViajesEnCurso.GetInt32();

                    if (json.TryGetProperty("totalViajesCancelados", out var totalViajesCancelados))
                        ViewBag.TotalViajesCancelados = totalViajesCancelados.GetInt32();
                }
            }
            catch
            {
                TempData["Error"] = "No se pudo cargar la información del dashboard.";
            }

            return View();
        }
    }
}