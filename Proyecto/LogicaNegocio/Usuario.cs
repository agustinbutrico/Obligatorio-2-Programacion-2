using LogicaNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio
{
    public class Usuario : IValidate
    {
        #region Atributos de la clase
        private int _id;
        private static int s_ultId = 0; // Inicializado con el id siguiente a la ultima precarga
        private string _nombre = string.Empty; // Inicializado con una cadena vacía
        private string _apellido = string.Empty; // Inicializado con una cadena vacía
        private string _email = string.Empty; // Inicializado con una cadena vacía
        private string _contrasenia = string.Empty; // Inicializado con una cadena vacía
        #endregion

        #region Propiedades
        public int Id
        {
            get { return _id; }  // Solo lectura, asignado internamente.
        }
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = EvaluarNombre(value); }
        }
        public string Apellido
        {
            get { return _apellido; }
            set { _apellido = EvaluarApellido(value); }
        }
        public string Email
        {
            get { return _email; }
            set { _email = EvaluarEmail(value); }
        }
        public string Contrasenia
        {
            get { return _contrasenia; }
            set { _contrasenia = EvaluarContrasenia(value); }
        }
        #endregion

        #region Constructor
        public Usuario(string nombre, string apellido, string email, string contrasenia)
        {
            _id = Usuario.s_ultId; // Asigna el ID único
            Usuario.s_ultId++; // Incrementa el ID único
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Contrasenia = contrasenia;
        }
        #endregion

        #region Validación
        // Evaluaciones
        private static string EvaluarNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                throw new ArgumentException("El nombre no puede ser vacío");
            }
            return nombre;
        }
        private static string EvaluarApellido(string apellido)
        {
            if (string.IsNullOrEmpty(apellido))
            {
                throw new ArgumentException("El apellido no puede ser vacío");
            }
            return apellido;
        }
        private static string EvaluarEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("El email no puede ser vacío");
            }
            if (email.IndexOf('@') == -1)
            {
                throw new ArgumentException("El email debe pertenecer a un domino (debe tener @)");
            }
            return email;
        }
        private static string EvaluarContrasenia(string contrasenia)
        {
            if (string.IsNullOrEmpty(contrasenia))
            {
                throw new ArgumentException("El contrasenia no puede ser vacío");
            }
            if (contrasenia.Length < 8)
            {
                throw new ArgumentException("La contraseña debe tener al menos 8 caracteres");
            }
            if (!contrasenia.Any(char.IsLetter))
            {
                throw new ArgumentException("La contraseña debe incluir al menos una letra");
            }
            if (!contrasenia.Any(char.IsDigit))
            {
                throw new ArgumentException("La contraseña debe incluir al menos un número");
            }
            return contrasenia;
        }

        // Validación de Usuario, es virtual ya que le hereda a otras clases
        public virtual void Validar()
        {
        }
        #endregion

        #region Método Equals
        // Sobre escritura del metodo Equals que es usado por Contains
        public override bool Equals(object? obj)
        {
            if (obj != null && obj is Usuario)
            {
                Usuario usuario = (Usuario)obj;
                return Nombre == usuario.Nombre && Apellido == usuario.Apellido;
            }
            return false;
        }
        #endregion
    }
}
