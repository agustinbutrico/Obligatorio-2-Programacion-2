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
        public IActionResult SaleDetails(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                // Almacena en una variable la venta activa
                Publicacion? venta = sistema.ObtenerPublicacionPorId(id, true, false);
                // Casteo exlpícito de Venta
                Venta? ventaActiva = (Venta?)venta;
                // Almacena la venta en una variable temporal
                if (ventaActiva != null)
                {
                    ViewBag.Venta = ventaActiva;
                }

                // Obtiene el rol actual del usuario
                string? currentRole = HttpContext.Session.GetString("UserRole");
                if (currentRole == "Cliente")
                {
                    // Obtiene el id del Usuario con el dato almacenado al realizar el login
                    int idUser = HttpContext.Session.GetInt32("UserId") ?? 0;
                    // Almacena en una variable el usuario activo
                    Usuario? cliente = sistema.ObtenerUsuarioPorId(idUser, true, false);
                    // Casteo explícito de Cliente
                    Cliente? clienteActivo = (Cliente?)cliente;
                    // Almacena el cliente en una variable temporal
                    if (clienteActivo != null)
                    {
                        ViewBag.Cliente = clienteActivo;
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult SaleDetails(int ventaId, string action)
        {
            try
            {
                // Obtiene el rol actual del usuario
                string? currentRole = HttpContext.Session.GetString("UserRole");

                // Almacena en una variable la venta activa
                Publicacion? venta = sistema.ObtenerPublicacionPorId(ventaId, true, false);
                // Casteo exlpícito de Venta
                Venta? ventaActiva = (Venta?)venta;
                
                if (ventaActiva != null)
                {
                    // Almacena la venta en una variable
                    ViewBag.Venta = ventaActiva;

                    // Logica para la compra de Publicaciones de tipo Venta
                    if (currentRole == "Cliente" && action == "BuySale")
                    {
                        // Obtiene el id del Usuario con el dato almacenado al realizar el login
                        int idUser = HttpContext.Session.GetInt32("UserId") ?? 0;
                        // Almacena en una variable el usuario activo
                        Usuario? cliente = sistema.ObtenerUsuarioPorId(idUser, true, false);
                        // Casteo explícito de Cliente
                        Cliente? clienteActivo = (Cliente?)cliente;

                        if (clienteActivo != null)
                        {
                            // Almacena el cliente en una variable
                            ViewBag.Cliente = clienteActivo;

                            // Almacena en una variable el precio de venta
                            decimal precioVenta = sistema.ConsultarPrecioVenta(ventaActiva, ventaActiva.Articulos);

                            // Manejo de errores
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
                                    // Cambia de estado la venta y registra el Cliente que la compró
                                    // Cobra el valor de la venta al usuario activo
                                    sistema.CompraVenta(clienteActivo, ventaActiva);

                                    // Mensaje de Confirmación
                                    ViewBag.Confirmacion = "La compra fue registrada correctamente, la misma debe ser autorizada por un administrador";
                                }
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Mensaje = $"{ex.Message}";
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
                ViewBag.Mensaje = $"{ex.Message}";
            }

            // Si algo falla, permanece en la vista
            return View();
        }

        [HttpGet]
        public IActionResult AuctionDetails(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                // Almacena en una variable la subasta activa
                Publicacion? subasta = sistema.ObtenerPublicacionPorId(id, false, true);
                // Casteo explícito a Subasta
                Subasta? subastaActiva = (Subasta?)subasta;
                // Crear el modelo con la Subasta
                if (subastaActiva != null)
                {
                    ViewBag.Subasta = subastaActiva;
                };

                // Obtiene el rol actual del usuario
                string? currentRole = HttpContext.Session.GetString("UserRole");
                if (currentRole == "Cliente")
                {
                    // Obtiene el id del Usuario con el dato almacenado al realizar el login
                    int idUser = HttpContext.Session.GetInt32("UserId") ?? 0;
                    // Almacena en una variable el usuario activo
                    Usuario? cliente = sistema.ObtenerUsuarioPorId(idUser, true, false);
                    // Casteo explícito de Cliente
                    Cliente? clienteActivo = (Cliente?)cliente;
                    // Almacena el cliente en una variable temporal
                    if (clienteActivo != null)
                    {
                        ViewBag.Cliente = clienteActivo;
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult AuctionDetails(int subastaId, decimal monto, string action)
        {
            try
            {
                // Obtiene el rol actual del usuario
                string? currentRole = HttpContext.Session.GetString("UserRole");

                // Almacena en una variable la subasta activa
                Publicacion? subasta = sistema.ObtenerPublicacionPorId(subastaId, false, true);
                // Casteo explícito a Subasta
                Subasta? subastaActiva = (Subasta?)subasta;

                if (subastaActiva != null)
                {
                    // Almacena la subastaActiva en una variable temporal
                    ViewBag.Subasta = subastaActiva;

                    // Logica para la Oferta del Cliente en una Publicacion de tipo Subasta
                    if (currentRole == "Cliente" && action == "OfferAuction")
                    {
                        // Obtiene el id del Usuario con el dato almacenado al realizar el login
                        int idUser = HttpContext.Session.GetInt32("UserId") ?? 0;
                        // Almacena en una variable el usuario activo
                        Usuario? cliente = sistema.ObtenerUsuarioPorId(idUser, true, false);
                        // Casteo explícito de Cliente
                        Cliente? clienteActivo = (Cliente?)cliente;

                        if (clienteActivo != null)
                        {
                            // Almacena el cliente en una variable temporal
                            ViewBag.Cliente = clienteActivo;

                            // Almacena en una variable la oferta más alta
                            decimal mejorOferta = 0;
                            // Almacena en una variable si el cliente realizo una oferta para esta subasta anteriormente
                            bool ofertoAnteriormente = false;

                            for (int i = 0; i < subastaActiva.Ofertas.Count; i++)
                            {
                                // Calcula la oferta más alta
                                if (subastaActiva.Ofertas[i].Monto > mejorOferta)
                                {
                                    mejorOferta = subastaActiva.Ofertas[i].Monto;
                                }
                                // Verifica si el cliente actual es el mismo que el usuario de la oferta
                                if (clienteActivo.Nombre == subastaActiva.Ofertas[i].Usuario?.Nombre)
                                {
                                    ofertoAnteriormente = true;
                                }
                            }

                            // Manejo de errores
                            if (monto <= 0)
                            {
                                ViewBag.Mensaje = "El monto debe ser mayor que 0";
                            }
                            else if (monto % 1 != 0)
                            {
                                ViewBag.Mensaje = "El monto debe ser un número entero";
                            }
                            else if (monto > clienteActivo.Saldo)
                            {
                                ViewBag.Mensaje = "Saldo insuficiente";
                            }
                            else if (monto <= mejorOferta)
                            {
                                ViewBag.Mensaje = "El monto debe ser mayor que la mejor oferta";
                            }
                            else if (ofertoAnteriormente)
                            {
                                ViewBag.Mensaje = "Solo está permitido ofertar una vez por subasta";
                            }
                            else
                            {
                                // Crea la nueva oferta para la subasta actual
                                sistema.AltaOferta(clienteActivo, subastaActiva, monto, DateTime.Now);

                                // Mensaje de Confirmación
                                ViewBag.Confirmacion = "La oferta fue registrada correctamente";
                            }
                        }
                    }
                    // Logica para el cierre de una Publicacion de tipo Subasta
                    else if (currentRole == "Administrador" && action == "CloseAuction")
                    {
                        // Obtiene el id del Usuario con el dato almacenado al realizar el login
                        int idUser = HttpContext.Session.GetInt32("UserId") ?? 0;
                        // Almacena en una variable el usuario activo
                        Usuario? administrador = sistema.ObtenerUsuarioPorId(idUser, false, true);
                        // Casteo explícito de Administrador
                        Administrador? administradorActivo = (Administrador?)administrador;

                        if (administradorActivo != null)
                        {
                            // Almacena el administrador en una variable
                            ViewBag.Administrador = administradorActivo;

                            // Manejo de errores
                            if (subastaActiva?.Estado.ToUpper() != "ABIERTA")
                            {
                                ViewBag.Mensaje = "La subasta no se encuentra activa";
                            }
                            else if (subastaActiva.Ofertas.Count == 0)
                            {
                                ViewBag.Mensaje = "No hay ofertas realizadas para esta subasta";
                            }
                            else
                            {
                                // Cambia de estado la subasta y registra el cliente que la ganó
                                // Registra el Administrador que cerro la subasta y la fecha fin
                                sistema.CompraSubasta(administradorActivo, subastaActiva);

                                // Mensaje de Confirmación
                                ViewBag.Confirmacion = "La subasta fue cerrada correctamente";
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Mensaje = $"{ex.Message}";
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
                ViewBag.Mensaje = $"{ex.Message}";
            }

            // Si algo falla, permanece en la vista
            return View();
        }
    }
}
