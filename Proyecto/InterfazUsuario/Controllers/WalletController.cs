using Microsoft.AspNetCore.Mvc;

namespace InterfazUsuario.Controllers
{
    public class WalletController : Controller
    {
        [HttpGet]
        public IActionResult Funds()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddFunds()
        {
            return View();
        }
    }
}
