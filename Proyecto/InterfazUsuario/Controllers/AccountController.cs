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
                // Manejo de errores
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contrasenia))
                {
                    ViewBag.Mensaje = "No pueden haber campos vacios";
                }
                else if (email.IndexOf('@') == -1)
                {
                    ViewBag.Mensaje = "Debe incluir el dominio del email";
                }
                else if (contrasenia.Length < 8)
                {
                    ViewBag.Mensaje = "La contrasenia debe ser de al menos 8 digitos";
                }
                else if (!contrasenia.Any(char.IsLetter))
                {
                    ViewBag.Mensaje = "La contrasenia debe tener al menos una letra";
                }
                else if (!contrasenia.Any(char.IsDigit))
                {
                    ViewBag.Mensaje = "La contrasenia debe tener al menos un dígito";
                }
                else
                {
                    // Comprobar si el usuario existe
                    if (sistema.ObtenerUsuarioPorEmailYContrasenia(email, contrasenia, false, false) != null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
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
