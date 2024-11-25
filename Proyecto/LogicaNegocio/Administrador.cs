using LogicaNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio
{
    public class Administrador : Usuario
    {
        #region Constructor
        public Administrador(string nombre, string apellido, string email, string contrasenia)
            : base(nombre, apellido, email, contrasenia) // Llamada al constructor de la clase base (Usuario)
        {
        }
        #endregion

        #region Validación
        // Validación de Administrador, hereda de Usuario
        public override void Validar()
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
