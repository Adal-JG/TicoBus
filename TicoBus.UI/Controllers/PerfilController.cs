using Microsoft.AspNetCore.Mvc;
using TicoBus.UI.ApiClients;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class PerfilController : BaseController
    {
        private readonly PerfilApiClient _perfilApiClient;

        public PerfilController(PerfilApiClient perfilApiClient)
        {
            _perfilApiClient = perfilApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var validacion = ValidarSesion();

            if (validacion != null)
            {
                return validacion;
            }

            var usuarioId = ObtenerUsuarioId();

            if (usuarioId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var usuario = await _perfilApiClient.ObtenerUsuario(usuarioId.Value);

            if (usuario == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var model = new PerfilViewModel
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                Rol = usuario.Rol.ToString(),
                FechaCreacion = usuario.FechaCreacion
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(PerfilViewModel model)
        {
            var validacion = ValidarSesion();

            if (validacion != null)
            {
                return validacion;
            }

            var usuario = await _perfilApiClient.ObtenerUsuario(model.Id);

            if (usuario == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!ModelState.IsValid)
            {
                model.NombreUsuario = usuario.NombreUsuario;
                model.NombreCompleto = usuario.NombreCompleto;
                model.Rol = usuario.Rol.ToString();
                model.FechaCreacion = usuario.FechaCreacion;

                return View(model);
            }

            var correoResponse = await _perfilApiClient.ActualizarCorreo(model.Id, model.Correo);

            if (correoResponse == null || !correoResponse.Exitoso)
            {
                ModelState.AddModelError("", correoResponse?.Mensaje ?? "No se pudo actualizar el perfil.");
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(model.NuevaClave))
            {
                if (string.IsNullOrWhiteSpace(model.ClaveActual))
                {
                    ModelState.AddModelError("", "Debe ingresar la clave actual.");

                    model.NombreUsuario = usuario.NombreUsuario;
                    model.NombreCompleto = usuario.NombreCompleto;
                    model.Rol = usuario.Rol.ToString();
                    model.FechaCreacion = usuario.FechaCreacion;

                    return View(model);
                }

                var claveResponse = await _perfilApiClient.CambiarClave(
                    usuario.NombreUsuario,
                    model.ClaveActual,
                    model.NuevaClave);

                if (claveResponse == null || !claveResponse.Exitoso)
                {
                    ModelState.AddModelError("", claveResponse?.Mensaje ?? "No se pudo cambiar la clave.");

                    model.NombreUsuario = usuario.NombreUsuario;
                    model.NombreCompleto = usuario.NombreCompleto;
                    model.Rol = usuario.Rol.ToString();
                    model.FechaCreacion = usuario.FechaCreacion;

                    return View(model);
                }
            }

            HttpContext.Session.SetString("Nombre", usuario.NombreCompleto);

            TempData["Exito"] = "Perfil actualizado correctamente.";

            return RedirectToAction("Index");
        }
    }
}