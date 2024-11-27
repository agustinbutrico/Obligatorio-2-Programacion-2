using InterfazUsuario.Models;
using LogicaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace InterfazUsuario.Controllers
{
    public class PublicationsController : Controller
    {
        // Se llama a la instancia con patron singleton
        private Sistema sistema = Sistema.Instancia;

        [HttpGet]
        public IActionResult Offers()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Publications()
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
    }
}
