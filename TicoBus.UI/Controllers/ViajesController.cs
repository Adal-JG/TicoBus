using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicoBus.Model;
using TicoBus.UI.ApiClients;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class ViajesController : BaseController
    {
        private readonly ViajesApiClient _viajesApiClient;
        private readonly RutasApiClient _rutasApiClient;
        private readonly UnidadesApiClient _unidadesApiClient;
        private readonly ChoferesApiClient _choferesApiClient;

        public ViajesController(
            ViajesApiClient viajesApiClient,
            RutasApiClient rutasApiClient,
            UnidadesApiClient unidadesApiClient,
            ChoferesApiClient choferesApiClient)
        {
            _viajesApiClient = viajesApiClient;
            _rutasApiClient = rutasApiClient;
            _unidadesApiClient = unidadesApiClient;
            _choferesApiClient = choferesApiClient;
        }

        public async Task<IActionResult> Index(string? filtro)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            ViewBag.Filtro = filtro;

            var viajes = await _viajesApiClient.Listar(filtro);

            return View(viajes);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            await CargarListas();

            return View(new ViajeViewModel
            {
                FechaHoraSalida = RedondearMinutos(DateTime.Now),
                FechaHoraLlegadaEstimada = RedondearMinutos(DateTime.Now.AddHours(2))
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ViajeViewModel model)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            if (!ModelState.IsValid)
            {
                await CargarListas();
                return View(model);
            }

            var viaje = ConvertirAEntidad(model);

            var response = await _viajesApiClient.Agregar(viaje);

            if (response == null || !response.Exitoso)
            {
                ModelState.AddModelError("", response?.Mensaje ?? "No se pudo registrar el viaje.");
                await CargarListas();
                return View(model);
            }

            TempData["Exito"] = response.Mensaje;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var viaje = await _viajesApiClient.ObtenerPorId(id);

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

            await CargarListas();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ViajeViewModel model)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            if (!ModelState.IsValid)
            {
                await CargarListas();
                return View(model);
            }

            var viaje = ConvertirAEntidad(model);

            var response = await _viajesApiClient.Actualizar(model.Id, viaje);

            if (response == null || !response.Exitoso)
            {
                ModelState.AddModelError("", response?.Mensaje ?? "No se pudo actualizar el viaje.");
                await CargarListas();
                return View(model);
            }

            TempData["Exito"] = response.Mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Iniciar(int id)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var response = await _viajesApiClient.Iniciar(id);

            TempData[response != null && response.Exitoso ? "Exito" : "Error"] =
                response?.Mensaje ?? "No se pudo iniciar el viaje.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Cancelar(int id)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var viaje = await _viajesApiClient.ObtenerPorId(id);

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
        public async Task<IActionResult> Cancelar(ViajeViewModel model)
        {
            var validacion = ValidarRol("Administrador", "Chofer");

            if (validacion != null)
            {
                return validacion;
            }

            var response = await _viajesApiClient.Cancelar(model.Id, model.MotivoCancelacion ?? "");

            if (response == null || !response.Exitoso)
            {
                ModelState.AddModelError("", response?.Mensaje ?? "No se pudo cancelar el viaje.");
                return View(model);
            }

            TempData["Exito"] = response.Mensaje;
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

        private async Task CargarListas()
        {
            var rutas = await _rutasApiClient.Listar(null);
            var unidades = await _unidadesApiClient.Listar(null);
            var choferes = await _choferesApiClient.Listar(null);

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

            ViewBag.Unidades = new SelectList(unidades, "Id", "Placa");

            ViewBag.Choferes = new SelectList(
                choferes.Select(c => new
                {
                    c.Id,
                    NombreCompleto = $"{c.Nombre} {c.Apellidos}"
                }),
                "Id",
                "NombreCompleto");
        }

        private DateTime RedondearMinutos(DateTime fecha)
        {
            return new DateTime(
                fecha.Year,
                fecha.Month,
                fecha.Day,
                fecha.Hour,
                fecha.Minute,
                0
            );
        }
    }
}