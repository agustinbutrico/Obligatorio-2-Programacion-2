using LogicaNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio
{
    #pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Articulo : IValidate
    #pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        #region  Atributos de la clase
        private int _id;
        private static int s_ultId = 0; // Inicializado con el id siguiente a la ultima precarga
        private string _nombre = string.Empty; // Inicializado con una cadena vacía
        private decimal _precio = 0; // Inicializado con 0
        private string _categoria = string.Empty; // Inicializado con una cadena vacía
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
        public decimal Precio
        {
            get { return _precio; }
            set { _precio = EvaluarPrecio(value); }
        }
        public string Categoria
        {
            get { return _categoria; }
            set { _categoria = EvaluarCategoria(value); }
        }
        #endregion

        #region Constructor
        public Articulo(string nombre, decimal precio, string categoria)
        {
            _id = Articulo.s_ultId; // Asigna el ID único
            Articulo.s_ultId++; // Incrementa el ID único
            Nombre = nombre;
            Precio = precio;
            Categoria = categoria;
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
        private static decimal EvaluarPrecio(decimal precio)
        {
            if (precio < 0)
            {
                throw new InvalidOperationException("El precio no puede ser negativo");
            }
            return precio;
        }
        private static string EvaluarCategoria(string categoria)
        {
            if (string.IsNullOrEmpty(categoria))
            {
                throw new ArgumentException("La categoria no puede ser vacía");
            }
            return categoria;
        }

        // Validación de Articulo
        public void Validar()
        {
        }
        #endregion

        #region Método Equals
        // Sobre escritura del metodo Equals que es usado por Contains
        public override bool Equals(object? obj)
        {
            if (obj != null && obj is Articulo)
            {
                Articulo articulo = (Articulo)obj;
                return Nombre == articulo.Nombre;
            }
            return false;
        }
        #endregion
    }
}
