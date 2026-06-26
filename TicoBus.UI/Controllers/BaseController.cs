using Microsoft.AspNetCore.Mvc;

namespace TicoBus.UI.Controllers
{
    public class BaseController : Controller
    {
        protected bool EstaAutenticado()
        {
            return HttpContext.Session.GetInt32("UsuarioId") != null;
        }

        protected string? ObtenerRol()
        {
            return HttpContext.Session.GetString("Rol");
        }

        protected int? ObtenerUsuarioId()
        {
            return HttpContext.Session.GetInt32("UsuarioId");
        }

        protected IActionResult ValidarSesion()
        {
            if (!EstaAutenticado())
            {
                return RedirectToAction("Index", "Login");
            }

            return null!;
        }

        protected IActionResult ValidarRol(params string[] rolesPermitidos)
        {
            var validacionSesion = ValidarSesion();

            if (validacionSesion != null)
            {
                return validacionSesion;
            }

            var rolActual = ObtenerRol();

            if (rolActual == null || !rolesPermitidos.Contains(rolActual))
            {
                return RedirectToAction("AccesoDenegado", "Home");
            }

            return null!;
        }
    }
}