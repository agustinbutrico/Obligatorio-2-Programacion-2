using InterfazUsuario.Models;
using LogicaNegocio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InterfazUsuario.Controllers
{
    public class PublicationsController : Controller
    {
        // Se llama a la instancia con patron singleton
        private Sistema sistema = Sistema.Instancia;

        [HttpGet]
        public IActionResult ListPublications()
        {
            // Almacena en una variable las ventas
            List<Publicacion> ventas = sistema.ObtenerPublicaciones(true, false);
            // Casteo explícito de ventas
            var listaVentas = ventas.OfType<Venta>().ToList();

            // Almacena en una variable las subastas
            List<Publicacion> subastas = sistema.ObtenerPublicaciones(false, true);
            // Casteo explícito de subastas
            var listaSubastas = subastas.OfType<Subasta>().ToList();

            // Crear el modelo con ambas listas
            var model = new ListPublicationsViewModel
            {
                Ventas = listaVentas,
                Subastas = listaSubastas
            };

            // Pasar el modelo a la vista
            return View(model);
        }

        [HttpGet]
        public IActionResult ListOffers(int id)
        {
            // Almacena en una variable la subasta activa
            Publicacion? subasta = sistema.ObtenerPublicacionPorId(id, false, true);
            // Casteo explícito a Subasta
            var subastaActiva = (Subasta?)subasta;

            // Crear el modelo con la Subasta
            var model = new ListOffersViewModel
            {
                Subasta = subastaActiva
            };

            // Pasar el modelo a la vista
            return View(model);
        }

        [HttpGet]
        public IActionResult Offert(int id)
        {
            // Almacena el ID de la subasta en la sesión
            HttpContext.Session.SetInt32("SubastaId", id);
            
            return View();
        }

        [HttpPost]
        public IActionResult Offert(decimal monto)
        {
            try
            {
                // Manejo de errores
                if (monto <= 0)
                {
                    ViewBag.Mensaje = "El monto debe ser mayor que 0";
                }
                else if (monto % 1 != 0)
                {
                    ViewBag.Mensaje = "El monto debe ser un número entero";
                }
                else
                {
                    // Obtiene el id del Usuario con el dato almacenado al realizar el login
                    int idUser = HttpContext.Session.GetInt32("UserId") ?? 0;
                    // Almacena en una variable el usuario activo
                    Usuario? cliente = sistema.ObtenerUsuarioPorId(idUser, true, false);
                    // Casteo explícito de Cliente
                    var clienteActivo = (Cliente?)cliente;

                    // Obtiene el id de la Publicacion con el dato almacenado al cargar la vista Offert
                    int currentIdSubasta = HttpContext.Session.GetInt32("SubastaId") ?? 0;
                    // Almacena en una variable la subasta activa
                    Publicacion? subasta = sistema.ObtenerPublicacionPorId(currentIdSubasta, false, true);
                    // Casteo explícito de Subasta
                    var subastaActiva = (Subasta?)subasta;

                    // Crea la nueva oferta para la subasta actual
                    sistema.AltaOferta(clienteActivo, subastaActiva, monto, DateTime.Now);


                    if (clienteActivo != null)
                    {
                        // Cobra el valor de la oferta al usuario activo
                        clienteActivo.Saldo -= monto;
                        ViewBag.Confirmacion = "La oferta fue registrada correctamente";
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Mensaje = $"Error de operación: {ex.Message}";
            }
            catch (ArgumentNullException ex)
            {
                ViewBag.Mensaje = $"{ex.Message}";
            }
            catch (ArgumentException ex)
            {
                ViewBag.Mensaje = $"{ex.Message}";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = $"Error inesperado: {ex.Message}";
            }

            // Si algo falla, permanece en la vista de login con el mensaje correspondiente
            return View();
        }

        [HttpGet]
        public IActionResult Buy(int id)
        {
            // Almacena el ID de la venta en la sesión
            HttpContext.Session.SetInt32("VentaId", id);

            return View();
        }

        [HttpPost]
        public IActionResult Buy()
        {
            try
            {
                // Obtiene el id del Usuario con el dato almacenado al realizar el login
                int idUser = HttpContext.Session.GetInt32("UserId") ?? 0;
                // Almacena en una variable el usuario activo
                Usuario? cliente = sistema.ObtenerUsuarioPorId(idUser, true, false);
                // Casteo explícito de Cliente
                var clienteActivo = (Cliente?)cliente;

                // Obtiene el id de la Publicacion con el dato almacenado al cargar la vista Buy
                int currentIdVenta = HttpContext.Session.GetInt32("VentaId") ?? 0;
                // Almacena en una variable la venta activa
                Publicacion? venta = sistema.ObtenerPublicacionPorId(currentIdVenta, true, false);
                // Casteo exlpícito de Venta
                var ventaActiva = (Venta?)venta;

                if (ventaActiva != null && clienteActivo != null)
                {
                    // Almacena en una variable el precio de venta de la venta
                    decimal precioVenta = sistema.ConsultarPrecioVenta(ventaActiva, ventaActiva.Articulos);

                    if (clienteActivo?.Saldo < precioVenta)
                    {
                        ViewBag.Mensaje = "Saldo insuficiente";
                    }
                    else if (ventaActiva?.Estado.ToUpper() != "ABIERTA")
                    {
                        ViewBag.Mensaje = "La venta no se encuentra activa";
                    }
                    else
                    {
                        if (clienteActivo != null)
                        {
                            clienteActivo.Saldo -= precioVenta;
                            ventaActiva.Cliente = clienteActivo;
                            ventaActiva.Estado = "CERRADA";
                            ViewBag.Confirmacion = "La compra fue registrada correctamente, la misma debe ser autorizada por un administrador";
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Mensaje = $"Error de operación: {ex.Message}";
            }
            catch (ArgumentNullException ex)
            {
                ViewBag.Mensaje = $"{ex.Message}";
            }
            catch (ArgumentException ex)
            {
                ViewBag.Mensaje = $"{ex.Message}";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = $"Error inesperado: {ex.Message}";
            }

            // Si algo falla, permanece en la vista de login con el mensaje correspondiente
            return View();
        }
    }
}
