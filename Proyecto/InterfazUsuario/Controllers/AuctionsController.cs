using InterfazUsuario.Models;
using LogicaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace InterfazUsuario.Controllers
{
    public class AuctionsController : Controller
    {
        // Se llama a la instancia con patron singleton
        private Sistema sistema = Sistema.Instancia;

        [HttpGet]
        public IActionResult ListAuctions()
        {
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                // Almacena en una variable las subastas
                List<Publicacion> subastas = sistema.ObtenerPublicaciones(false, true);
                // Casteo explícito de subastas
                List<Subasta> listaSubastas = subastas.OfType<Subasta>().ToList();
                // Ordenar por fecha (ascendente)
                List<Subasta> listaSubastasOrdenada = listaSubastas.OrderBy(p => p.Fecha).ToList();

                // Crear el modelo con ambas listas
                var model = new ListAuctionsViewModel
                {
                    Subastas = listaSubastasOrdenada
                };

                // Pasar el modelo a la vista
                return View(model);
            }
            return View();
        }
    }
}
