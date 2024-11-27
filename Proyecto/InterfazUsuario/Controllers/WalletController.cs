using InterfazUsuario.Models;
using LogicaNegocio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterfazUsuario.Controllers
{
    public class WalletController : Controller
    {
        // Se llama a la instancia con patron singleton
        private Sistema sistema = Sistema.Instancia;

        [HttpGet]
        public IActionResult Funds()
        {
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                // Obtiene el id del Usuario con el dato almacenado al realizar el login
                int idUser = HttpContext.Session.GetInt32("UserId") ?? 0;
                // Almacena en una variable el usuario activo
                Usuario? cliente = sistema.ObtenerUsuarioPorId(idUser, true, false);
                // Casteo explícito de Cliente
                var clienteActivo = (Cliente?)cliente;

                // Crear el modelo con ambas listas
                var model = new FundsViewModel
                {
                    Cliente = clienteActivo
                };

                // Pasar el modelo a la vista
                return View(model);
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddFunds()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddFunds(decimal saldo)
        {
            try
            {
                // Manejo de errores
                if (saldo <= 0)
                {
                    ViewBag.Mensaje = "El saldo a añadir debe ser mayor que 0";
                }
                else if (saldo % 1 != 0)
                {
                    ViewBag.Mensaje = "El saldo a añadir debe ser un número entero";
                }
                else
                {
                    // Obtiene el id del Usuario con el dato almacenado al realizar el login
                    int idUser = HttpContext.Session.GetInt32("UserId") ?? 0;
                    // Almacena en una variable el usuario activo
                    Usuario? cliente = sistema.ObtenerUsuarioPorId(idUser, true, false);
                    // Casteo explícito de Cliente
                    var clienteActivo = (Cliente?)cliente;

                    if (clienteActivo != null)
                    {
                        // Añade el saldo al usuario activo
                        clienteActivo.Saldo += saldo;
                        ViewBag.Confirmacion = "Saldo añadido correctamente correctamente";
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

            return View();
        }
    }
}
