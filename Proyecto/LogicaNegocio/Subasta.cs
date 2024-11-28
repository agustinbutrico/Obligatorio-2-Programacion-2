using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio
{
    #pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Subasta : Publicacion
    #pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        #region Atributos de la clase
        private Administrador? _administrador; // Inicializado con una instancia por defecto
        private List<Oferta> _ofertas = new List<Oferta>(); // Inicializado con una lista vacía
        #endregion

        #region Propiedades
        public Administrador? Administrador
        {
            get { return _administrador; }
            set { _administrador = value; }
        }
        public List<Oferta> Ofertas
        {
            get { return _ofertas; }
            set { _ofertas = value; }
        }
        #endregion

        #region Constructor
        public Subasta(string nombre, string estado, DateTime fecha, List<Articulo> articulos, Cliente? cliente, Administrador? administrador, DateTime fechaFin, List<Oferta> ofertas)
            : base(nombre, estado, fecha, articulos, cliente, fechaFin) // Llamada al constructor de la clase base (Publicacion)
        {
            Administrador = administrador;
            Ofertas = ofertas;
        }
        #endregion

        #region Validación
        // Validación de Subasta, hereda de Publicacion
        public override void Validar()
        {
        }
        #endregion

        #region Método Equals
        // Sobre escritura del metodo Equals que es usado por Contains
        public override bool Equals(object? obj)
        {
            if (obj != null && obj is Subasta)
            {
                Subasta subasta = (Subasta)obj;
                return Nombre == subasta.Nombre;
            }
            return false;
        }
        #endregion

        #region Alta
        public void AltaOferta(Usuario? usuario, decimal monto, DateTime fecha)
        {
            Oferta oferta = new Oferta(usuario, monto, fecha); // Crea una oferta con el costructor de Oferta
            if (!Ofertas.Contains(oferta)) // Utilizando el Equals de Oferta valida que un usuario no haga más de una oferta
            {
                Ofertas.Add(oferta); // Añade a la lista _ofertas
            }
        }
        #endregion
    }
}
