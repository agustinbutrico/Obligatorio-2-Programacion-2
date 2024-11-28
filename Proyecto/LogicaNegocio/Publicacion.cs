using LogicaNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio
{
    #pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Publicacion : IValidate
    #pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        #region Atributos de la clase
        private int _id;
        private static int s_ultId = 0; // Inicializado con el id siguiente a la ultima precarga
        private string _nombre = string.Empty; // Inicializado con una cadena vacía
        private string _estado = string.Empty; // Inicializado con una cadena vacía
        private DateTime _fecha = DateTime.Now; // Inicializado con la fecha actual
        private List<Articulo> _articulos = new List<Articulo>(); // Inicializado con una lista vacía
        private Cliente? _cliente; // Inicializado con una instancia por defecto
        private DateTime _fechaFin = DateTime.Now; // Inicializado con la fecha actual
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
        public string Estado
        {
            get { return _estado; }
            set { _estado = EvaluarEstado(value); }
        }
        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }
        public DateTime FechaFin
        {
            get { return _fechaFin; }
            set { _fechaFin = value; }
        }
        public List<Articulo> Articulos
        {
            get { return _articulos; }
            set { _articulos = value; }
        }
        public Cliente? Cliente
        {
            get { return _cliente; }
            set { _cliente = value; }
        }
        #endregion

        #region Constructor
        public Publicacion(string nombre, string estado, DateTime fecha, List<Articulo> articulos, Cliente? cliente, DateTime fechaFin)
        {
            _id = Publicacion.s_ultId; // Asigna el ID único
            Publicacion.s_ultId++; // Incrementa el ID único
            Nombre = nombre;
            Estado = estado;
            Fecha = fecha;
            Articulos = articulos;
            Cliente = cliente;
            FechaFin = fechaFin;
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
        private static string EvaluarEstado(string estado)
        {
            if (estado != "ABIERTA" && estado != "CERRADA" && estado != "CANCELADA")
            {
                throw new ArgumentException("El estado de la publicacion tiene que ser ABIERTA, CERRADA o CANCELADA");
            }
            return estado;
        }

        // Validación de Publicacion, es virtual ya que le hereda a otras clases
        public virtual void Validar()
        {
            if (FechaFin <= Fecha && FechaFin != DateTime.MinValue)
            {
                throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio.");
            }
        }
        #endregion

        #region Método Equals
        // Sobre escritura del metodo Equals que es usado por Contains
        public override bool Equals(object? obj)
        {
            if (obj != null && obj is Publicacion)
            {
                Publicacion publicacion = (Publicacion)obj;
                return Nombre == publicacion.Nombre;
            }
            return false;
        }
        #endregion
    }
}
