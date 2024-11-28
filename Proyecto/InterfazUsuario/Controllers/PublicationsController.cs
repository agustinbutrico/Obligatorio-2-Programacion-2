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
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                // Almacena en una variable las ventas
                List<Publicacion> ventas = sistema.ObtenerPublicaciones(true, false);
                // Casteo explícito de ventas
                List<Venta> listaVentas = ventas.OfType<Venta>().ToList();

                // Ordenar por fecha (ascendente)
                List<Venta> listaVentasOrdenada = listaVentas.OrderBy(p => p.Fecha).ToList();

                // Almacena en una variable las subastas
                List<Publicacion> subastas = sistema.ObtenerPublicaciones(false, true);
                // Casteo explícito de subastas
                List<Subasta> listaSubastas = subastas.OfType<Subasta>().ToList();

                // Ordenar por fecha (ascendente)
                List<Subasta> listaSubastasOrdenada = listaSubastas.OrderBy(p => p.Fecha).ToList();

                // Crear el modelo con ambas listas
                var model = new ListPublicationsViewModel
                {
                    Ventas = listaVentasOrdenada,
                    Subastas = listaSubastasOrdenada
                };

                // Pasar el modelo a la vista
                return View(model);
            }
            return View();
        }

        [HttpGet]
        public IActionResult ListOffers(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                // Almacena en una variable la subasta activa
                Publicacion? subasta = sistema.ObtenerPublicacionPorId(id, false, true);
                // Casteo explícito a Subasta
                Subasta? subastaActiva = (Subasta?)subasta;

                // Crear el modelo con la Subasta
                var model = new ListOffersViewModel
                {
                    Subasta = subastaActiva
                };

                // Pasar el modelo a la vista
                return View(model);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Offert(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                // Almacena el ID de la subasta en la sesión
                HttpContext.Session.SetInt32("SubastaId", id);
            }
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
                    Cliente? clienteActivo = (Cliente?)cliente;

                    // Obtiene el id de la Publicacion con el dato almacenado al cargar la vista Offert
                    int currentIdSubasta = HttpContext.Session.GetInt32("SubastaId") ?? 0;
                    // Almacena en una variable la subasta activa
                    Publicacion? subasta = sistema.ObtenerPublicacionPorId(currentIdSubasta, false, true);
                    // Casteo explícito de Subasta
                    Subasta? subastaActiva = (Subasta?)subasta;

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

            // Si algo falla, permanece en la vista
            return View();
        }

        [HttpGet]
        public IActionResult Buy(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                // Obtiene el id del Usuario con el dato almacenado al realizar el login
                int idUser = HttpContext.Session.GetInt32("User     Id") ?? 0;
                // Almacena en una variable el usuario activo
                Usuario? cliente = sistema.ObtenerUsuarioPorId(idUser, true, false);
                // Casteo explícito de Cliente
                Cliente? clienteActivo = (Cliente?)cliente;
                // Almacena el cliente en una variable
                if (clienteActivo != null)
                {
                    ViewBag.Cliente = clienteActivo;
                }

                // Almacena el ID de la venta en la sesión
                HttpContext.Session.SetInt32("VentaId", id);

                // Almacena en una variable la venta activa
                Publicacion? venta = sistema.ObtenerPublicacionPorId(id, true, false);
                // Casteo exlpícito de Venta
                Venta? ventaActiva = (Venta?)venta;
                // Almacena la venta en una variable
                if (ventaActiva != null)
                {
                    ViewBag.Venta = ventaActiva;
                }
            }
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
                Cliente? clienteActivo = (Cliente?)cliente;
                // Almacena el cliente en una variable
                if (clienteActivo != null)
                {
                    ViewBag.Cliente = clienteActivo;
                }

                // Obtiene el id de la Publicacion con el dato almacenado al cargar la vista Buy
                int currentIdVenta = HttpContext.Session.GetInt32("VentaId") ?? 0;
                // Almacena en una variable la venta activa
                Publicacion? venta = sistema.ObtenerPublicacionPorId(currentIdVenta, true, false);
                // Casteo exlpícito de Venta
                Venta? ventaActiva = (Venta?)venta;
                // Almacena la venta en una variable
                if (ventaActiva != null)
                {
                    ViewBag.Venta = ventaActiva;
                }

                if (ventaActiva != null && clienteActivo != null)
                {
                    // Almacena en una variable el precio de venta
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
                            ventaActiva.Estado = "PENDIENTE";
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

            // Si algo falla, permanece en la vista
            return View();
        }

        [HttpGet]
        public IActionResult CloseAuction(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                // Obtiene el id del Usuario con el dato almacenado al realizar el login
                int idUser = HttpContext.Session.GetInt32("UserId") ?? 0;
                // Almacena en una variable el usuario activo
                Usuario? administrador = sistema.ObtenerUsuarioPorId(idUser, false, true);
                // Casteo explícito de Administrador
                Administrador? administradorActivo = (Administrador?)administrador;
                // Almacena el administrador en una variable
                if (administradorActivo != null)
                {
                    ViewBag.Administrador = administradorActivo;
                }

                // Almacena el ID de la subasta en la sesión
                HttpContext.Session.SetInt32("SubastaId", id);

                // Almacena en una variable la subasta activa
                Publicacion? subasta = sistema.ObtenerPublicacionPorId(id, false, true);
                // Casteo exlpícito de Subasta
                Subasta? subastaActiva = (Subasta?)subasta;
                // Almacena la subasta en una variable
                if (subastaActiva != null)
                {
                    ViewBag.Subasta = subastaActiva;
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult CloseAuction()
        {
            try
            {
                // Obtiene el id del Usuario con el dato almacenado al realizar el login
                int idUser = HttpContext.Session.GetInt32("UserId") ?? 0;
                // Almacena en una variable el usuario activo
                Usuario? administrador = sistema.ObtenerUsuarioPorId(idUser, false, true);
                // Casteo explícito de Administrador
                Administrador? administradorActivo = (Administrador?)administrador;
                // Almacena el cliente en una variable
                if (administradorActivo != null)
                {
                    ViewBag.Administrador = administradorActivo;
                }

                // Obtiene el id de la Publicacion con el dato almacenado al cargar la vista Close
                int currentIdSubasta = HttpContext.Session.GetInt32("SubastaId") ?? 0;
                // Almacena en una variable la subasta activa
                Publicacion? subasta = sistema.ObtenerPublicacionPorId(currentIdSubasta, false, true);
                // Casteo exlpícito de Subasta
                Subasta? subastaActiva = (Subasta?)subasta;
                // Almacena la subasta en una variable
                if (subastaActiva != null)
                {
                    ViewBag.Subasta = subastaActiva;
                }

                if (subastaActiva != null && administradorActivo != null)
                {
                    // Almacena en una variable el precio de la mejor oferta en la subasta
                    // El monto de la ultima oferta realizada es la mejor oferta
                    decimal precioSubasta = subastaActiva.Ofertas[subastaActiva.Ofertas.Count -1].Monto;

                    if (subastaActiva?.Estado.ToUpper() != "ABIERTA")
                    {
                        ViewBag.Mensaje = "La subasta no se encuentra activa";
                    }
                    else
                    {
                        if (administradorActivo != null)
                        {
                            subastaActiva.Estado = "CERRADA";
                            subastaActiva.Administrador = administradorActivo;
                            subastaActiva.FechaFin = DateTime.Now;
                            ViewBag.Confirmacion = "La subasta fue cerrada correctamente";
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

            // Si algo falla, permanece en la vista
            return View();
        }
    }
}
