using LogicaNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio
{
    public class Venta : Publicacion
    {
        #region Atributos de la clase
        private bool _ofertaRelampago = false; // Inicializado en falso
        #endregion

        #region Propiedades
        public bool OfertaRelampago
        {
            get { return _ofertaRelampago; }
            set { _ofertaRelampago = value; }
        }
        #endregion

        #region Constructor
        public Venta(string nombre, string estado, DateTime fecha, List<Articulo> articulos, Cliente? cliente, Administrador? administrador, DateTime fechaFin, bool ofertaRelampago)
            : base(nombre, estado, fecha, articulos, cliente, administrador, fechaFin) // Llamada al constructor de la clase base (Publicacion)
        {
            OfertaRelampago = ofertaRelampago;
        }
        #endregion

        #region Validación
        // Validación de Venta, hereda de Publicacion
        public override void Validar()
        {
        }
        #endregion

        #region Método Equals
        // Sobre escritura del metodo Equals que es usado por Contains
        public override bool Equals(object? obj)
        {
            if (obj != null && obj is Venta)
            {
                Venta venta = (Venta)obj;
                return Nombre == venta.Nombre;
            }
            return false;
        }
        #endregion
    }
}
