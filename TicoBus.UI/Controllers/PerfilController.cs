using Microsoft.AspNetCore.Mvc;
using TicoBus.DA.Repositories;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class PerfilController : BaseController
    {
        private readonly UsuarioRepository _usuarioRepository;

        public PerfilController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public IActionResult Index()
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

            var usuario = _usuarioRepository.ObtenerPorId(usuarioId.Value);

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
        public IActionResult Index(PerfilViewModel model)
        {
            var validacion = ValidarSesion();

            if (validacion != null)
            {
                return validacion;
            }

            var usuario = _usuarioRepository.ObtenerPorId(model.Id);

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

            usuario.Correo = model.Correo;

            if (!string.IsNullOrWhiteSpace(model.NuevaClave))
            {
                if (string.IsNullOrWhiteSpace(model.ClaveActual))
                {
                    ModelState.AddModelError("", "Debe ingresar la clave actual.");
                    return View(model);
                }

                if (usuario.Clave != model.ClaveActual)
                {
                    ModelState.AddModelError("", "La clave actual no es correcta.");
                    return View(model);
                }

                usuario.Clave = model.NuevaClave;
            }

            _usuarioRepository.Actualizar(usuario);

            HttpContext.Session.SetString("Nombre", usuario.NombreCompleto);

            TempData["Exito"] = "Perfil actualizado correctamente.";

            return RedirectToAction("Index");
        }
    }
}