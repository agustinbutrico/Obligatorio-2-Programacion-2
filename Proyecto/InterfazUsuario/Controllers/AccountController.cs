using Microsoft.AspNetCore.Mvc;

namespace InterfazUsuario.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string contrasenia)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string name, string apellido, string email, string contrasenia)
        {
            return View();
        }
    }
}
