using Microsoft.AspNetCore.Mvc;
using TicoBus.BL.Interfaces;
using TicoBus.Model;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = _loginService.Login(model.NombreUsuario, model.Clave, out string mensaje);

            if (usuario == null)
            {
                ViewBag.Error = mensaje;
                return View(model);
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("Nombre", usuario.NombreCompleto);
            HttpContext.Session.SetString("Rol", usuario.Rol.ToString());

            return usuario.Rol switch
            {
                RolUsuario.Administrador => RedirectToAction("Index", "Dashboard"),
                RolUsuario.Chofer => RedirectToAction("Index", "Dashboard"),
                RolUsuario.Pasajero => RedirectToAction("Index", "MisViajes"),
                _ => RedirectToAction("Index", "Login")
            };
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}