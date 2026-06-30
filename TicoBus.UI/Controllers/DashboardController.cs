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

            if (rol == "Chofer" && usuarioId != null)
            {
                var datos = await _dashboardApiClient.ObtenerChofer(usuarioId.Value);

                if (datos != null)
                {
                    ViewBag.ViajesHoyJson = datos.Value.GetProperty("viajesHoy").GetRawText();
                    ViewBag.ProximoViajeJson = datos.Value.GetProperty("proximoViaje").GetRawText();
                    ViewBag.TotalViajesHoy = datos.Value.GetProperty("totalViajesHoy").GetInt32();
                    ViewBag.PasajerosRegistrados = datos.Value.GetProperty("pasajerosRegistrados").GetInt32();
                    ViewBag.AsientosDisponibles = datos.Value.GetProperty("asientosDisponibles").GetInt32();
                }

                return View("Chofer");
            }

            var admin = await _dashboardApiClient.ObtenerAdmin();

            if (admin != null)
            {
                ViewBag.TotalChoferes = admin.Value.GetProperty("totalChoferes").GetInt32();
                ViewBag.TotalPasajeros = admin.Value.GetProperty("totalPasajeros").GetInt32();
                ViewBag.TotalRutas = admin.Value.GetProperty("totalRutas").GetInt32();
                ViewBag.TotalUnidades = admin.Value.GetProperty("totalUnidades").GetInt32();
                ViewBag.TotalViajesProgramados = admin.Value.GetProperty("totalViajesProgramados").GetInt32();
                ViewBag.TotalViajesEnCurso = admin.Value.GetProperty("totalViajesEnCurso").GetInt32();
                ViewBag.TotalViajesCancelados = admin.Value.GetProperty("totalViajesCancelados").GetInt32();
            }

            return View();
        }
    }
}