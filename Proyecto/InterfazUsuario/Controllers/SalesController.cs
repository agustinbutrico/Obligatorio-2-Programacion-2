using InterfazUsuario.Models;
using LogicaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace InterfazUsuario.Controllers
{
    public class SalesController : Controller
    {
        // Se llama a la instancia con patron singleton
        private Sistema sistema = Sistema.Instancia;

        [HttpGet]
        public IActionResult ListSales()
        {
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                // Almacena en una variable las ventas
                List<Publicacion> ventas = sistema.ObtenerPublicaciones(true, false);
                // Casteo explícito de ventas
                List<Venta> listaVentas = ventas.OfType<Venta>().ToList();
                // Ordenar por fecha (ascendente)
                List<Venta> listaVentasOrdenada = listaVentas.OrderBy(p => p.Fecha).ToList();

                // Crear el modelo con ambas listas
                var model = new ListSalesViewModel
                {
                    Ventas = listaVentasOrdenada
                };

                // Pasar el modelo a la vista
                return View(model);
            }
            return View();
        }
    }
}
