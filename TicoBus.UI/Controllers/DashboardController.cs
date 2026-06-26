using Microsoft.AspNetCore.Mvc;

namespace TicoBus.UI.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.Rol = HttpContext.Session.GetString("Rol");

            return View();
        }
    }
}