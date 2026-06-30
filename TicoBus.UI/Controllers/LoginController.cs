using Microsoft.AspNetCore.Mvc;
using TicoBus.UI.ApiClients;
using TicoBus.UI.Models;

namespace TicoBus.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthApiClient _authApiClient;

        public LoginController(AuthApiClient authApiClient)
        {
            _authApiClient = authApiClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultado = await _authApiClient.Login(model.NombreUsuario, model.Clave);

            if (resultado == null || !resultado.Exitoso || resultado.UsuarioId == null)
            {
                ViewBag.Error = resultado?.Mensaje ?? "Credenciales incorrectas.";
                return View(model);
            }

            HttpContext.Session.SetInt32("UsuarioId", resultado.UsuarioId.Value);
            HttpContext.Session.SetString("Nombre", resultado.Nombre ?? "");
            HttpContext.Session.SetString("Rol", resultado.Rol ?? "");

            if (resultado.Rol == "Pasajero")
            {
                return RedirectToAction("Index", "MisViajes");
            }

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}