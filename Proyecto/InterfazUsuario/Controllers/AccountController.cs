using LogicaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace InterfazUsuario.Controllers
{
    public class AccountController : Controller
    {
        private Sistema sistema = Sistema.Instancia;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string contrasenia)
        {
            try
            {
                // Validar entrada de usuario de manera básica
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contrasenia) && contrasenia.Length >= 8)
                {
                    // Comprobar si el usuario no existe
                    if (sistema.ObtenerUsuarioPorEmailYContrasenia(email, contrasenia, false, false) != null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // Agregar mensaje si la validación falla
                    ViewBag.Mensaje = "Por favor, ingresa un email y una contraseña válidos.";
                }
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Mensaje = $"Error de operación: {ex.Message}";
            }
            catch (ArgumentNullException ex)
            {
                ViewBag.Mensaje = $"Falta un argumento obligatorio: {ex.Message}";
            }
            catch (ArgumentException ex)
            {
                ViewBag.Mensaje = $"Argumento inválido: {ex.Message}";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = $"Error inesperado: {ex.Message}";
            }

            // Si algo falla, permanece en la vista de login con el mensaje correspondiente
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string nombre, string apellido, string email, string contrasenia)
        {
            return View();
        }
    }
}
