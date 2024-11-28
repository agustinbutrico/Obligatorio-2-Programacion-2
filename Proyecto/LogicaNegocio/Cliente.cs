using LogicaNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio
{
    #pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Cliente : Usuario, IValidate
    #pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        #region Atributos de la clase
        private decimal _saldo = 0; // Inicializado en 0
        #endregion

        #region Propiedades
        public decimal Saldo
        {
            get { return _saldo; }
            set { _saldo = EvaluarSaldo(value); }
        }
        #endregion

        #region Constructor
        public Cliente(string nombre, string apellido, string email, string contrasenia, decimal saldo)
           : base(nombre, apellido, email, contrasenia) // Llamada al constructor de la clase base (Usuario)
        {
            Saldo = saldo;
        }
        #endregion

        #region Validación
        // Evaluaciones
        private static decimal EvaluarSaldo(decimal saldo)
        {
            if (saldo < 0)
            {
                throw new InvalidOperationException("El saldo no puede ser negativo");
            }
            return saldo;
        }

        // Validación de Cliente, hereda de Usuario
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
