using LogicaNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LogicaNegocio
{
    public class Oferta : IValidate
    {
        #region Atributos de la clase
        private int _id;
        private static int s_ultId = 0; // Inicializado con el id siguiente a la ultima precarga
        private Usuario? _usuario; // Inicializado con una instancia por defecto
        private decimal _monto = 0; // Inicializado con 0
        private DateTime _fecha = DateTime.Now; // Inicializado con la fecha actual

        private static decimal s_ultimoMonto = 0;  // Almacena el último monto ingresado

        #endregion

        #region Propiedades
        public int Id
        {
            get { return _id; }  // Solo lectura, asignado internamente.
        }
        public Usuario? Usuario
        {
            get { return _usuario; }
            set { _usuario = EvaluarUsuario(value); }
        }
        public decimal Monto
        {
            get { return _monto; }
            set { _monto = EvaluarMonto(value); }
        }
        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }
        #endregion

        #region Constructor
        public Oferta(Usuario? usuario, decimal monto, DateTime fecha)
        {
            _id = Oferta.s_ultId; // Asigna el ID único
            Oferta.s_ultId++; // Incrementa el ID único
            Usuario = usuario;
            Monto = monto;
            Fecha = fecha;
        }
        #endregion

        #region Validación
        // Evaluaciones
        private static Usuario EvaluarUsuario(Usuario? usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException("No se encontró un usuario correspondiente a los datos proporcionados");
            }
            return usuario;
        }
        private static decimal EvaluarMonto(decimal monto)
        {
            if (monto <= 0)
            {
                throw new InvalidOperationException("El monto no puede ser menor a 0");
            }
            if (monto % 1 != 0)
            {
                throw new InvalidOperationException("El monto debe ser un número entero");
            }
            if (monto <= s_ultimoMonto)
            {
                throw new InvalidOperationException($"El monto ingresado debe ser mayor a la mejor oferta");
            }
            s_ultimoMonto = monto; // Actualiza el último monto registrado
            return monto;
        }
        // Validación de Oferta
        public void Validar()
        {
        }
        #endregion

        #region Método Equals
        // Sobre escritura del metodo Equals que es usado por Contains
        public override bool Equals(object? obj)
        {
            if (obj != null && obj is Oferta)
            {
                Oferta oferta = (Oferta)obj;
                return Usuario == oferta.Usuario;
            }
            return false;
        }
        #endregion
    }
}

