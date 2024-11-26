using InterfazUsuario.Models;
using LogicaNegocio;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InterfazUsuario.Controllers
{
    public class HomeController : Controller
    {
        // Se llama a la instancia con patron singleton
        private Sistema sistema = Sistema.Instancia;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Publicacion> ventas = sistema.ObtenerPublicaciones(true, false);
            List<Publicacion> subastas = sistema.ObtenerPublicaciones(false, true);

            // Casteo explícito a Venta o Subasta
            var ListaVentas = ventas.OfType<Venta>().ToList();
            var ListaSubastas = subastas.OfType<Subasta>().ToList();

            // Crear el modelo con ambas listas
            var model = new PublicacionesViewModel
            {
                Ventas = ListaVentas,
                Subastas = ListaSubastas
            };

            // Pasar el modelo a la vista
            return View(model);
        }

        public IActionResult IndexAdmin()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
