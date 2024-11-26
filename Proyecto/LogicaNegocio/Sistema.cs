using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LogicaNegocio
{
    public class Sistema
    {
        #region Patron singleton
        private static Sistema _instancia; // Instancia única
        private static readonly object _bloqueo = new object(); // Para hilos seguros

        // Propiedad para acceder a la instancia única
        public static Sistema Instancia
        {
            get
            {
                // Double-check locking para garantizar seguridad en multithreading
                if (_instancia == null)
                {
                    lock (_bloqueo)
                    {
                        if (_instancia == null)
                        {
                            _instancia = new Sistema();
                        }
                    }
                }
                return _instancia;
            }
        }
        #endregion

        #region Constructor
        // Atributos de la clase con propiedades automaticas (shortHand)
        private List<Usuario> _usuarios {  get; set; }
        private List<Publicacion> _publicaciones { get; set; }
        private List<Articulo> _articulos { get; set; }

        // Ejecucion principal
        private Sistema()
        {
            _usuarios = new List<Usuario>();
            _publicaciones = new List<Publicacion>();
            _articulos = new List<Articulo>();
            PrecargaArticulo();
            PrecargaUsuario();
            PrecargarPublicacion();
            PrecargaOferta();
        }
        #endregion

        /// <summary>
        /// El Parseo de datos sirve para modificar los datos ingresados por el usuario
        /// o modificar datos para mostrarlos al usuario.
        /// La funcion ParseoId permite obtener una lista de Ids a partir de una cadena de texto
        /// La funcion ParseoArticulo permite obtener una cadena de texto a partir de una lista de articulos
        /// </summary>
        #region Utilidades
        #region Universal
        // Convierte en una lista de ids el string pasado por parametros
        public List<int> ParseoInt(string intsCrudos)
        {
            List<int> lista_ints = new List<int>(); // Crea una lista de los ids ingresados
            string[] ids = intsCrudos.Split(','); // Crea un array de los ids

            for (int i = 0; i < ids.Length; i++) // Recorre todos los elementos de ids
            {
                lista_ints.Add(int.Parse(ids[i].Trim())); // Remueve los espacios, transforma a int y añade a la lista el id
            }
            return lista_ints;
        }
        // Convierte en una lista de nombres el string pasado por parametros
        public List<string> ParseoString(string stringsCrudos)
        {
            List<string> listaStrings = new List<string>(); // Crea una lista de los nombres ingresados
            string[] nombres = stringsCrudos.Split(','); // Crea un array de los nombres

            for (int i = 0; i < nombres.Length; i++) // Recorre todos los elementos de nombres
            {
                listaStrings.Add(nombres[i].Trim()); // Remueve los espaciosy añade a la lista el nombre
            }
            return listaStrings;
        }
        #endregion
        #region Articulo
        // Convierte en string los ids de la lista de articulos pasada por parametros
        public string ParseoArticulo(List<Articulo> articulos)
        {
            string ids_articulos = string.Empty;
            for (int i = 0; i < articulos.Count; i++)
            {
                ids_articulos += $"{articulos[i].Id}, ";
            }

            if (articulos.Count > 0)
            {
                // Quitamos la , del final de los ids
                ids_articulos = ids_articulos.Substring(0, ids_articulos.Length - 2);
            }
            return ids_articulos;
        }
        #endregion
        #region Oferta
        // Convierte en string los ids de la lista de ofertas pasada por parametros
        public string ParseoOferta(List<Oferta> ofertas)
        {
            string ids_ofertas = string.Empty;
            for (int i = 0; i < ofertas.Count; i++)
            {
                ids_ofertas += $"{ofertas[i].Id}, ";
            }

            if (ofertas.Count > 0)
            {
                // Quitamos la , del final de los ids
                ids_ofertas = ids_ofertas.Substring(0, ids_ofertas.Length - 2);
            }
            return ids_ofertas;
        }
        #endregion
        #endregion

        /// <summary>
        /// Las lista son utilizadas en todas las funciones de impresión.
        /// Por ejemplo si queremos imprimir clientes, debemos pasarle a la función
        /// imprimirUsuario una lista de clientes.
        /// Esta lista se puede conseguir con:
        /// ObtenerCliente, almacena en una lista todos los usuarios que sean clientes
        /// obtenerClientePorId, almacena en una lista los clientes de ids determinados
        /// obtenerClientePorNombre, almacena en una lista los clientes de nombres determinados
        /// </summary>
        #region Obtención de listas
        #region Articulo
        public List<Articulo> ObtenerArticulos()
        {
            bool hayArticulo = false;
            List<Articulo> articulos = new List<Articulo>();  // Inicializamos la lista que contendrá los artículos
            for (int i = 0; i < _articulos.Count; i++)
            {
                hayArticulo = true;
                articulos.Add(_articulos[i]); // Se añade cualquier artículo a la lista artículos
            }
            if (!hayArticulo)
            {
                throw new ArgumentException("No hay ningún artículo en el sistema");
            }
            return articulos;
        }
        public List<Articulo> ObtenerArticuloPorId(List<int> ids)
        {
            bool hayArticulo = false;
            List<Articulo> articulos = new List<Articulo>();  // Inicializamos la lista que contendrá los artículos
            for (int i = 0; i < _articulos.Count; i++)
            {
                if (ids.Contains(_articulos[i].Id)) // Si la lista de ids contiene algún artículo
                {
                    hayArticulo = true;
                    articulos.Add(_articulos[i]); // Se añade el artículo a la lista artículos
                }
            }
            if (!hayArticulo)
            {
                throw new ArgumentException("No hay ningún artículo con los ids proporcionados");
            }
            return articulos;
        }
        public List<Articulo> ObtenerArticuloPorNombre(List<string> nombres)
        {
            bool hayArticulo = false;
            List<Articulo> articulos = new List<Articulo>();  // Inicializamos la lista que contendrá los artículos
            for (int i = 0; i < _articulos.Count; i++)
            {
                if (nombres.Contains(_articulos[i].Nombre)) // Si la lista de nombres contiene algún artículo
                {
                    hayArticulo = true;
                    articulos.Add(_articulos[i]); // Se añade el artículo a la lista artículos
                }
            }
            if (!hayArticulo)
            {
                throw new ArgumentException("No hay ningún artículo con los nombres proporcionados");
            }
            return articulos;
        }
        public List<Articulo> ObtenerArticuloPorCategoria(List<string> categorias)
        {
            bool hayArticulo = false;
            List<Articulo> articulos = new List<Articulo>();  // Inicializamos la lista que contendrá los artículos
            for (int i = 0; i < _articulos.Count; i++)
            {
                if (categorias.Contains(_articulos[i].Categoria)) // Si la lista de nombres contiene algún artículo
                {
                    hayArticulo = true;
                    articulos.Add(_articulos[i]); // Se añade el artículo a la lista artículos
                }
            }
            if (!hayArticulo)
            {
                throw new ArgumentException("No hay ningún artículo con las catergoría proporcionada");
            }
            return articulos;
        }
        #endregion
        #region Publicacion
        public List<Publicacion> ObtenerPublicaciones(bool esUnicamenteVenta, bool esUnicamenteSubasta)
        {
            bool hayVenta = false;
            bool haySubasta = false;
            List<Publicacion> publicaciones = new List<Publicacion>();  // Inicializamos la lista que contendrá las publicaciones
            for (int i = 0; i < _publicaciones.Count; i++)
            {
                if ((esUnicamenteVenta && !esUnicamenteSubasta) || (!esUnicamenteVenta && !esUnicamenteSubasta))
                {
                    if (_publicaciones[i] is Venta venta)
                    {
                        hayVenta = true;
                        publicaciones.Add(venta); // Se añade la venta a la lista publicaciones
                    }
                }
                if ((esUnicamenteSubasta && !esUnicamenteVenta) || (!esUnicamenteVenta && !esUnicamenteSubasta))
                {
                    if (_publicaciones[i] is Subasta subasta)
                    {
                        haySubasta = true;
                        publicaciones.Add(subasta); // Se añade la subasta a la lista publicaciones
                    }
                }
            }
            if (!hayVenta && !haySubasta)
            {
                throw new ArgumentException("No hay ningúna pubicación en el sistema");
            }
            if (!hayVenta && esUnicamenteVenta)
            {
                throw new ArgumentException("No hay ningúna venta en el sistema");
            } 
            if (!haySubasta && esUnicamenteSubasta)
            {
                throw new ArgumentException("No hay ningúna subasta en el sistema");
            }
            return publicaciones;
        }
        public List<Publicacion> ObtenerPublicacionPorId(List<int> ids, bool esUnicamenteVenta, bool esUnicamenteSubasta)
        {
            bool hayVenta = false;
            bool haySubasta = false;
            List<Publicacion> publicaciones = new List<Publicacion>();  // Inicializamos la lista que contendrá las publicaciones
            for (int i = 0; i < _publicaciones.Count; i++)
            {
                if (ids.Contains(_publicaciones[i].Id)) // Si la lista de ids contiene algúna publicacion
                {
                    if ((esUnicamenteVenta && !esUnicamenteSubasta) || (!esUnicamenteVenta && !esUnicamenteSubasta))
                    {
                        if (_publicaciones[i] is Venta venta)
                        {
                            hayVenta = true;
                            publicaciones.Add(venta); // Se añade la venta a la lista publicaciones
                        }
                    }
                    if ((esUnicamenteSubasta && !esUnicamenteVenta) || (!esUnicamenteVenta && !esUnicamenteSubasta))
                    {
                        if (_publicaciones[i] is Subasta subasta)
                        {
                            haySubasta = true;
                            publicaciones.Add(subasta); // Se añade la subasta a la lista publicaciones
                        }
                    }
                }
            }
            if (!hayVenta && !haySubasta)
            {
                throw new ArgumentException("No hay publicaciones con los ids proporcionados");
            }
            if (!hayVenta && esUnicamenteVenta)
            {
                throw new ArgumentException("No hay ventas con los ids proporcionados");
            }
            if (!haySubasta && esUnicamenteSubasta)
            {
                throw new ArgumentException("No hay subastas con los ids proporcionados");
            }
            return publicaciones;
        }
        public Publicacion? ObtenerPublicacionPorId(int id, bool esUnicamenteVenta, bool esUnicamenteSubasta)
        {
            bool hayVenta = false;
            bool haySubasta = false;
            Publicacion? publicacion = null;
            int indice = 0;
            while (indice < _publicaciones.Count && !hayVenta && !haySubasta)
            {
                if (id == _publicaciones[indice].Id) // Si la lista de ids contiene algúna publicación
                {
                    if ((esUnicamenteVenta && !esUnicamenteSubasta) || (!esUnicamenteVenta && !esUnicamenteSubasta))
                    {
                        if (_publicaciones[indice] is Venta venta)
                        {
                            hayVenta = true;
                            publicacion = venta; // Se asigna la publicación
                        }
                    }
                    if ((esUnicamenteSubasta && !esUnicamenteVenta) || (!esUnicamenteVenta && !esUnicamenteSubasta))
                    {
                        if (_publicaciones[indice] is Subasta subasta)
                        {
                            haySubasta = true;
                            publicacion = subasta; // Se asigna la publicación
                        }
                    }
                }
                indice++;
            }
            if (!hayVenta && !haySubasta)
            {
                throw new ArgumentException("No hay ningúna publicación con el id proporcionado");
            }
            if (!hayVenta && esUnicamenteVenta)
            {
                throw new ArgumentException("No hay ningúna venta con el id proporcionado");
            }
            if (!haySubasta && esUnicamenteSubasta)
            {
                throw new ArgumentException("No hay ningúna subasta con el id proporcionado");
            }
            return publicacion;
        }
        public List<Publicacion> ObtenerPublicacionPorNombre(List<string> nombres, bool esUnicamenteVenta, bool esUnicamenteSubasta)
        {
            bool hayVenta = false;
            bool haySubasta = false;
            List<Publicacion> publicaciones = new List<Publicacion>();  // Inicializamos la lista que contendrá las publicaciones
            for (int i = 0; i < _publicaciones.Count; i++)
            {
                if (nombres.Contains(_publicaciones[i].Nombre)) // Si la lista de nombres contiene algúna publicación
                {
                    if ((esUnicamenteVenta && !esUnicamenteSubasta) || (!esUnicamenteVenta && !esUnicamenteSubasta))
                    {
                        if (_publicaciones[i] is Venta venta)
                        {
                            hayVenta = true;
                            publicaciones.Add(venta); // Se añade la venta a la lista publicaciones
                        }
                    }
                    if ((esUnicamenteSubasta && !esUnicamenteVenta) || (!esUnicamenteVenta && !esUnicamenteSubasta))
                    {
                        if (_publicaciones[i] is Subasta subasta)
                        {
                            haySubasta = true;
                            publicaciones.Add(subasta); // Se añade la subasta a la lista publicaciones
                        }
                    }
                }
            }
            if (!hayVenta && !haySubasta)
            {
                throw new ArgumentException("No hay publicaciones con los nombres proporcionados");
            }
            if (!hayVenta && esUnicamenteVenta)
            {
                throw new ArgumentException("No hay ventas con los nombres proporcionados");
            }
            if (!haySubasta && esUnicamenteSubasta)
            {
                throw new ArgumentException("No hay subastas con los nombres proporcionados");
            }
            return publicaciones;
        }
        public Publicacion? ObtenerPublicacionPorNombre(string nombre, bool esUnicamenteVenta, bool esUnicamenteSubasta)
        {
            bool hayVenta = false;
            bool haySubasta = false;
            Publicacion? publicacion = null;
            int indice = 0;
            while (indice < _publicaciones.Count && !hayVenta && !haySubasta)
            {
                if (nombre.Contains(_publicaciones[indice].Nombre)) // Si la lista de nombres contiene algúna publicación
                {
                    if ((esUnicamenteVenta && !esUnicamenteSubasta) || (!esUnicamenteVenta && !esUnicamenteSubasta))
                    {
                        if (_publicaciones[indice] is Venta venta)
                        {
                            hayVenta = true;
                            publicacion = venta; // Se asigna la publicación
                        }
                    }
                    if ((esUnicamenteSubasta && !esUnicamenteVenta) || (!esUnicamenteVenta && !esUnicamenteSubasta))
                    {
                        if (_publicaciones[indice] is Subasta subasta)
                        {
                            haySubasta = true;
                            publicacion = subasta; // Se asigna la publicación
                        }
                    }
                }
                indice++;
            }
            if (!hayVenta && !haySubasta)
            {
                throw new ArgumentException("No hay ningúna publicación con el nombre proporcionado");
            }
            if (!hayVenta && esUnicamenteVenta)
            {
                throw new ArgumentException("No hay ningúna venta con el nombre proporcionado");
            }
            if (!haySubasta && esUnicamenteSubasta)
            {
                throw new ArgumentException("No hay ningúna subasta con el nombre proporcionado");
            }
            return publicacion;
        }
        public List<Publicacion> ObtenerPublicacionPorFecha(DateTime fechaInicio, DateTime fechaFin,bool esUnicamenteVenta, bool esUnicamenteSubasta)
        {
            bool hayVenta = false;
            bool haySubasta = false;
            List<Publicacion> publicaciones = new List<Publicacion>();  // Inicializamos la lista que contendrá las publicaciones
            for (int i = 0; i < _publicaciones.Count; i++)
            {
                if (_publicaciones[i].Fecha > fechaInicio && _publicaciones[i].Fecha < fechaFin) // Si la lista de nombres contiene algúna publicación
                {
                    if ((esUnicamenteVenta && !esUnicamenteSubasta) || (!esUnicamenteVenta && !esUnicamenteSubasta))
                    {
                        if (_publicaciones[i] is Venta venta)
                        {
                            hayVenta = true;
                            publicaciones.Add(venta); // Se asigna la publicación
                        }
                    }
                    if ((esUnicamenteSubasta && !esUnicamenteVenta) || (!esUnicamenteVenta && !esUnicamenteSubasta))
                    {
                        if (_publicaciones[i] is Subasta subasta)
                        {
                            haySubasta = true;
                            publicaciones.Add(subasta); // Se asigna la publicación
                        }
                    }
                }
            }
            if (!hayVenta && !haySubasta)
            {
                throw new ArgumentException("No hay publicaciones entre las fechas proporcionadas");
            }
            if (!hayVenta && esUnicamenteVenta)
            {
                throw new ArgumentException("No hay ventas entre las fechas proporcionadas");
            }
            if (!haySubasta && esUnicamenteSubasta)
            {
                throw new ArgumentException("No hay subastas entre las fechas proporcionadas");
            }
            return publicaciones;
        }
        #endregion
        #region Usuario
        public List<Usuario> ObtenerUsuarios(bool esUnicamenteCliente, bool esUnicamenteAdministrador)
        {
            bool hayCliente = false;
            bool hayAdministrador = false;
            List<Usuario> usuarios = new List<Usuario>();  // Inicializamos la lista que contendrá los usuarios
            for (int i = 0; i < _usuarios.Count; i++)
            {
                if ((esUnicamenteCliente && !esUnicamenteAdministrador) || (!esUnicamenteCliente && !esUnicamenteAdministrador))
                {
                    if (_usuarios[i] is Cliente cliente)
                    {
                        hayCliente = true;
                        usuarios.Add(cliente); // Se añade el cliente a la lista usuarios
                    }
                }
                if ((esUnicamenteAdministrador && !esUnicamenteCliente) || (!esUnicamenteCliente && !esUnicamenteAdministrador))
                {
                    if (_usuarios[i] is Administrador administrador)
                    {
                        hayAdministrador = true;
                        usuarios.Add(administrador); // Se añade el administrador a la lista usuarios
                    }
                }
            }
            if (!hayCliente && !hayAdministrador)
            {
                throw new ArgumentException("No hay ningún usuario en el sistema");
            }
            if (!hayCliente && esUnicamenteCliente)
            {
                throw new ArgumentException("No hay ningún cliente en el sistema");
            }
            if (!hayAdministrador && esUnicamenteAdministrador)
            {
                throw new ArgumentException("No hay ningún administrador en el sistema");
            }
            return usuarios;
        }
        public List<Usuario> ObtenerUsuarioPorId(List<int> ids, bool esUnicamenteCliente, bool esUnicamenteAdministrador)
        {
            bool hayCliente = false;
            bool hayAdministrador = false;
            List<Usuario> usuarios = new List<Usuario>();  // Inicializamos la lista que contendrá los usuarios
            for (int i = 0; i < _usuarios.Count; i++)
            {
                if (ids.Contains(_usuarios[i].Id)) // Si la lista de ids contiene algún usuario
                {
                    if ((esUnicamenteCliente && !esUnicamenteAdministrador) || (!esUnicamenteCliente && !esUnicamenteAdministrador))
                    {
                        if (_usuarios[i] is Cliente cliente)
                        {
                            hayCliente = true;
                            usuarios.Add(cliente); // Se añade el cliente a la lista usuarios
                        }
                    }
                    if ((esUnicamenteAdministrador && !esUnicamenteCliente) || (!esUnicamenteCliente && !esUnicamenteAdministrador))
                    {
                        if (_usuarios[i] is Administrador administrador)
                        {
                            hayAdministrador = true;
                            usuarios.Add(administrador); // Se añade el administrador a la lista usuarios
                        }
                    }
                }
            }
            if (!hayCliente && !hayAdministrador)
            {
                throw new ArgumentException("No hay usuarios con los ids proporcionados");
            }
            if (!hayCliente && esUnicamenteCliente)
            {
                throw new ArgumentException("No hay clientes con los ids proporcionados");
            }
            if (!hayAdministrador && esUnicamenteAdministrador)
            {
                throw new ArgumentException("No hay administradores con los ids proporcionados");
            }
            return usuarios;
        }
        public Usuario? ObtenerUsuarioPorId(int id, bool esUnicamenteCliente, bool esUnicamenteAdministrador)
        {
            bool hayCliente = false;
            bool hayAdministrador = false;
            Usuario? usuario = null;
            int indice = 0;
            while (indice < _usuarios.Count && !hayCliente && !hayAdministrador)
            {
                if (id == _usuarios[indice].Id) // Si la lista de ids contiene algúna usuario
                {
                    if ((esUnicamenteCliente && !esUnicamenteAdministrador) || (!esUnicamenteCliente && !esUnicamenteAdministrador))
                    {
                        if (_usuarios[indice] is Cliente cliente)
                        {
                            hayCliente = true;
                            usuario = cliente; // Se asigna el usuario
                        }
                    }
                    if ((esUnicamenteAdministrador && !esUnicamenteCliente) || (!esUnicamenteCliente && !esUnicamenteAdministrador))
                    {
                        if (_usuarios[indice] is Administrador administrador)
                        {
                            hayAdministrador = true;
                            usuario = administrador; // Se asigna el usuario
                        }
                    }
                }
                indice++;
            }
            if (!hayCliente && !hayAdministrador)
            {
                throw new ArgumentException("No hay ningún usuario con el id proporcionado");
            }
            if (!hayCliente && esUnicamenteCliente)
            {
                throw new ArgumentException("No hay ningún cliente con el id proporcionado");
            }
            if (!hayAdministrador && esUnicamenteAdministrador)
            {
                throw new ArgumentException("No hay ningún administrador con el id proporcionado");
            }
            return usuario;
        }
        public List<Usuario> ObtenerUsuarioPorNombre(List<string> nombres, bool esUnicamenteCliente, bool esUnicamenteAdministrador)
        {
            bool hayCliente = false;
            bool hayAdministrador = false;
            List<Usuario> usuarios = new List<Usuario>();  // Inicializamos la lista que contendrá los usuarios
            for (int i = 0; i < _usuarios.Count; i++)
            {
                if (nombres.Contains(_usuarios[i].Nombre)) // Si la lista de nombres contiene algún usuario
                {
                    if ((esUnicamenteCliente && !esUnicamenteAdministrador) || (!esUnicamenteCliente && !esUnicamenteAdministrador))
                    {
                        if (_usuarios[i] is Cliente cliente)
                        {
                            hayCliente = true;
                            usuarios.Add(cliente); // Se añade el cliente a la lista usuarios
                        }
                    }
                    if ((esUnicamenteAdministrador && !esUnicamenteCliente) || (!esUnicamenteCliente && !esUnicamenteAdministrador))
                    {
                        if (_usuarios[i] is Administrador administrador)
                        {
                            hayAdministrador = true;
                            usuarios.Add(administrador); // Se añade el administrador a la lista usuarios
                        }
                    }
                }
            }
            if (!hayCliente && !hayAdministrador)
            {
                throw new ArgumentException("No hay usuarios con los nombres proporcionados");
            }
            if (!hayCliente && esUnicamenteCliente)
            {
                throw new ArgumentException("No hay clientes con los nombres proporcionados");
            }
            if (!hayAdministrador && esUnicamenteAdministrador)
            {
                throw new ArgumentException("No hay administradores con los nombres proporcionados");
            }
            return usuarios;
        }
        public Usuario? ObtenerUsuarioPorNombre(string nombre, bool esUnicamenteCliente, bool esUnicamenteAdministrador)
        {
            bool hayCliente = false;
            bool hayAdministrador = false;
            Usuario? usuario = null;
            int indice = 0;
            while (indice < _usuarios.Count && !hayCliente && !hayAdministrador)
            {
                if (nombre.Contains(_usuarios[indice].Nombre)) // Si la lista de nombres contiene algún usuario
                {
                    if ((esUnicamenteCliente && !esUnicamenteAdministrador) || (!esUnicamenteCliente && !esUnicamenteAdministrador))
                    {
                        if (_usuarios[indice] is Cliente cliente)
                        {
                            hayCliente = true;
                            usuario = cliente; // Se asigna el usuario
                        }
                    }
                    if ((esUnicamenteAdministrador && !esUnicamenteCliente) || (!esUnicamenteCliente && !esUnicamenteAdministrador))
                    {
                        if (_usuarios[indice] is Administrador administrador)
                        {
                            hayAdministrador = true;
                            usuario = administrador; // Se asigna el usuario
                        }
                    }
                }
                indice++;
            }
            if (!hayCliente && !hayAdministrador)
            {
                throw new ArgumentException("No hay ningún usuario con el nombre proporcionado");
            }
            if (!hayCliente && esUnicamenteCliente)
            {
                throw new ArgumentException("No hay ningún cliente con el nombre proporcionado");
            }
            if (!hayAdministrador && esUnicamenteAdministrador)
            {
                throw new ArgumentException("No hay ningún administrador con el nombre proporcionado");
            }
            return usuario;
        }
        public Usuario? ObtenerUsuarioPorEmailYContrasenia(string email, string contrasenia, bool esUnicamenteCliente, bool esUnicamenteAdministrador)
        {
            bool hayCliente = false;
            bool hayAdministrador = false;
            Usuario? usuario = null;
            int indice = 0;
            while (indice < _usuarios.Count && !hayCliente && !hayAdministrador)
            {
                if (email.Contains(_usuarios[indice].Email) && contrasenia.Contains(_usuarios[indice].Contrasenia)) // Si la lista de email y contraseñas contiene algún usuario
                {
                    if ((esUnicamenteCliente && !esUnicamenteAdministrador) || (!esUnicamenteCliente && !esUnicamenteAdministrador))
                    {
                        if (_usuarios[indice] is Cliente cliente)
                        {
                            hayCliente = true;
                            usuario = cliente; // Se asigna el usuario
                        }
                    }
                    if ((esUnicamenteAdministrador && !esUnicamenteCliente) || (!esUnicamenteCliente && !esUnicamenteAdministrador))
                    {
                        if (_usuarios[indice] is Administrador administrador)
                        {
                            hayAdministrador = true;
                            usuario = administrador; // Se asigna el usuario
                        }
                    }
                }
                indice++;
            }
            if (!hayCliente && !hayAdministrador)
            {
                throw new ArgumentException("No hay ningún usuario con el email y contraseña proporcionados");
            }
            if (!hayCliente && esUnicamenteCliente)
            {
                throw new ArgumentException("No hay ningún cliente con el email y contraseña proporcionados");
            }
            if (!hayAdministrador && esUnicamenteAdministrador)
            {
                throw new ArgumentException("No hay ningún administrador con el email y contraseña proporcionados");
            }
            return usuario;
        }
        #endregion
        #region Oferta
        public List<Oferta> ObtenerOfertas(Publicacion? publicacion)
        {
            bool hayOferta = false;
            List<Oferta> ofertas = new List<Oferta>();  // Inicializamos la lista que contendrá las ofertas
            if (publicacion is Subasta subasta)
            {
                hayOferta = true;
                ofertas = subasta.Ofertas; // Se añade cualquier oferta a la lista ofertas
            }
            if (!hayOferta)
            {
                throw new ArgumentException("La subasta no contiene ofertas");
            }
            return ofertas;
        }
        #endregion
        #endregion

        /// <summary>
        /// Las funciones de alta se encargan de llamar a los constructores de las
        /// diferentes clases y pasar los parametros obtenidos en Program.
        /// </summary>
        #region Altas
        #region Articulo
        public void AltaArticulo(string nombre, decimal precio, string categoria)
        {
            Articulo nuevoArticulo = new Articulo(nombre, precio, categoria);
            // Validación de la relacion entre los datos ingresados
            nuevoArticulo.Validar();
            // Si los datos son validos entonces se registra el Articulo
            if (!_articulos.Contains(nuevoArticulo))
            {
                _articulos.Add(nuevoArticulo);
            }
            else
            {
                throw new ArgumentException("Ya existe un articulo registrado con el nombre proporcionado");
            }
        }
        #endregion
        #region Publicacion
        public void AltaPublicacion(string nombre, string estado, DateTime fecha, List<Articulo> articulos, Cliente? cliente, Administrador? administrador, DateTime fechaFin)
        {
            Publicacion nuevaPublicacion = new Publicacion(nombre, estado, fecha, articulos, cliente, administrador, fechaFin);
            // Validación de la relacion entre los datos ingresados
            nuevaPublicacion.Validar();
            // Si los datos son validos entonces se registra la Publicación
            if (!_publicaciones.Contains(nuevaPublicacion))
            {
                _publicaciones.Add(nuevaPublicacion);
            }
            else
            {
                throw new ArgumentException("Ya existe una publicacion registrada con el nombre proporcionado");
            }
        }
        public void AltaVenta(string nombre, string estado, DateTime fecha, List<Articulo> articulos, Cliente? cliente, Administrador? administrador, DateTime fechaFin, bool ofertaRelampago)
        {
            Venta nuevaVenta = new Venta(nombre, estado, fecha, articulos, cliente, administrador, fechaFin, ofertaRelampago);
            // Validación de la relacion entre los datos ingresados
            nuevaVenta.Validar();
            // Si los datos son validos entonces se registra la Venta
            if (!_publicaciones.Contains(nuevaVenta))
            {
                _publicaciones.Add(nuevaVenta);
            }
            else
            {
                throw new ArgumentException("Ya existe una publicacion registrada con el nombre proporcionado");
            }
        }
        public void AltaSubasta(string nombre, string estado, DateTime fecha, List<Articulo> articulos, Cliente? cliente, Administrador? administrador, DateTime fechaFin, List<Oferta> ofertas)
        {
            Subasta nuevaSubasta = new Subasta(nombre, estado, fecha, articulos, cliente, administrador, fechaFin, ofertas);
            // Validación de la relacion entre los datos ingresados
            nuevaSubasta.Validar();
            // Si los datos son validos entonces se registra la Subasta
            if (!_publicaciones.Contains(nuevaSubasta))
            {
                _publicaciones.Add(nuevaSubasta);
            }
            else
            {
                throw new ArgumentException("Ya existe una publicacion registrada con el nombre proporcionado");
            }
        }
        #endregion
        #region Usuario
        public void AltaUsuario(string nombre, string apellido, string email, string contrasenia)
        {
            Usuario nuevoUsuario = new Usuario(nombre, apellido, email, contrasenia);
            // Validación de la relacion entre los datos ingresados
            nuevoUsuario.Validar();
            // Si los datos son validos entonces se registra el Usuario
            if (!_usuarios.Contains(nuevoUsuario))
            {
                _usuarios.Add(nuevoUsuario);
            }
            else
            {
                throw new ArgumentException("Ya existe un usuario registrado con el nombre, apellido e email proporcionados");
            }
        }
        public void AltaCliente(string nombre, string apellido, string email, string contrasenia, decimal saldo)
        {
            Cliente nuevoCliente = new Cliente(nombre, apellido, email, contrasenia, saldo);
            // Validación de la relacion entre los datos ingresados
            nuevoCliente.Validar();
            // Si los datos son validos entonces se registra el Cliente
            if (!_usuarios.Contains(nuevoCliente))
            {
                _usuarios.Add(nuevoCliente);
            }
            else
            {
                throw new ArgumentException("Ya existe un usuario registrado con el nombre, apellido e email proporcionados");
            }
        }
        public void AltaAdministrador(string nombre, string apellido, string email, string contrasenia)
        {
            Administrador nuevoAdministrador = new Administrador(nombre, apellido, email, contrasenia);
            // Validación de la relacion entre los datos ingresados
            nuevoAdministrador.Validar();
            // Si los datos son validos entonces se registra el Administrador
            if (!_usuarios.Contains(nuevoAdministrador))
            {
                _usuarios.Add(nuevoAdministrador);
            }
            else
            {
                throw new ArgumentException("Ya existe un usuario registrado con el nombre, apellido e email proporcionados");
            }
        }
        #endregion
        #region Ofertas
        public void AltaOferta(Usuario? usuario, Publicacion? publicacion, decimal monto, DateTime fecha)
        {
            if (publicacion != null && publicacion is Subasta subasta) 
            {
                subasta.AltaOferta(usuario, monto, fecha);
            }
            else if (publicacion == null)
            {
                throw new ArgumentNullException("No fue posible acceder a la subasta proporcionada");
            }
            else 
            {
                throw new ArgumentException("No fue posible realizar la oferta debido a que no fue posible identificar la subasta");
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Las funciones de consulta tienen el objetivo de obtener datos calculados.
        /// Por ejemplo ConsultarPrecioVentaDeListaVenta obtiene los precios de las ventas buscadas.
        /// Este es un dato calculado ya que es necesario acceder a la venta y sumar el precio de todos sus articulos.
        /// </summary>
        #region Consultas
        public decimal ConsultarPrecioVenta(Publicacion? publicacion, List<Articulo> articulos)
        {
            decimal precio = 0;
            for (int i = 0; i < articulos.Count; i++)
            {
                precio += articulos[i].Precio;
            }
            if (publicacion is Venta venta)
            {
                if (venta.OfertaRelampago)
                {
                    // se aplica descuento si corresponde a la venta en especifico
                    precio = precio * 80 / 100;
                }
            }
            return precio;
        }
        public bool HayCliente()
        {
            for (int i = 0; i < _usuarios.Count; i++)
            {
                if (_usuarios[i] is Cliente cliente)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        /// <summary>
        /// Las precargas son relizadas a travez de las funciones de alta,
        /// esto se hace de este modo para que el id autoincremental se asigne correctamente
        /// </summary>
        #region Precargas
        #region Articulo
        private void PrecargaArticulo()
        {
            AltaArticulo("Pelota de football", 450, "Football");
            AltaArticulo("Camiseta deportiva", 1200, "Deporte");
            AltaArticulo("Zapatillas treking", 3500, "Treking");
            AltaArticulo("Raqueta de tenis", 4200, "Tenis");
            AltaArticulo("Balón de basquetball", 800, "Basquetball");
            AltaArticulo("Guantes de boxeo", 2200, "Boxeo");
            AltaArticulo("Casco de ciclismo", 1800, "Ciclismo");
            AltaArticulo("Saco de dormir", 2300, "Camping");
            AltaArticulo("Bolsa de gimnasio", 950, "Boxeo");
            AltaArticulo("Bicicleta de montaña", 15000, "Ciclismo");
            AltaArticulo("Mochila de trekking", 2100, "Treking");
            AltaArticulo("Protector solar", 320, "Playa");
            AltaArticulo("Botella térmica", 750, "Camping");
            AltaArticulo("Palo de hockey", 1700, "Hokey");
            AltaArticulo("Pesas ajustables", 3000, "Gimnasio");
            AltaArticulo("Cinta para correr", 25000, "Gimnasio");
            AltaArticulo("Guantes de arquero", 900, "Arquería");
            AltaArticulo("Tabla de surf", 12000, "Surf");
            AltaArticulo("Canilleras", 600, "Football");
            AltaArticulo("Traje de neopreno", 5400, "Surf");
            AltaArticulo("Gafas de natación", 650, "Natación");
            AltaArticulo("Bola de bowling", 3500, "Bowling");
            AltaArticulo("Skateboard", 2400, "Skating");
            AltaArticulo("Patines en línea", 2900, "Patinaaje");
            AltaArticulo("Salvavidas", 1200, "Playa");
            AltaArticulo("Set de pesas", 4200, "Gimnasio");
            AltaArticulo("Cuerda para saltar", 300, "Gimnasio");
            AltaArticulo("Bicicleta de carrera", 18500, "Ciclismo");
            AltaArticulo("Tobilleras con peso", 850, "Gimnasio");
            AltaArticulo("Set de dardos", 400, "Juegos");
            AltaArticulo("Bate de baseball", 1900, "Baseball");
            AltaArticulo("Bola de volleyball", 850, "Volleyball");
            AltaArticulo("Aro de basquetball", 2700, "Basquetball");
            AltaArticulo("Zapatilla de ciclismo", 1900, "Ciclismo");
            AltaArticulo("Silla de camping", 1100, "Camping");
            AltaArticulo("Sombrilla", 1600, "Playa");
            AltaArticulo("Tienda de campaña", 8700, "Camping");
            AltaArticulo("Colchoneta de yoga", 1200, "Deporte");
            AltaArticulo("Barra de dominadas", 1900, "Gimnasio");
            AltaArticulo("Malla", 600, "Ciclismo");
            AltaArticulo("Reloj deportivo", 6500, "Deporte");
            AltaArticulo("Monopatín eléctrico", 18000, "Ciclismo");
            AltaArticulo("Kit de pesca", 3200, "Pesca");
            AltaArticulo("Bolsa de golf", 7600, "Golf");
            AltaArticulo("Raqueta de badminton", 1600, "Badminton");
            AltaArticulo("Patineta longboard", 3300, "Skating");
            AltaArticulo("Bola de rugby", 1050, "Rugby");
            AltaArticulo("Kit de snorkel", 1800, "Natacion");
            AltaArticulo("Camiseta de compresión", 1300, "Deporte");
            AltaArticulo("Gorra deportiva", 400, "Deporte");
            AltaArticulo("Balón medicinal", 2000, "Salud");
            AltaArticulo("Kit de arquería", 9800, "Arquería");
            AltaArticulo("Soga de escalada", 5600, "Escalada");
            AltaArticulo("Casco de ski", 3700, "Ski");
            AltaArticulo("Balde", 1050, "Playa");
            AltaArticulo("Gafas de ciclismo", 900, "Ciclismo");
        }
        #endregion
        #region Publicacion
        private void PrecargarPublicacion()
        {
            AltaVenta("Verano en la playa", "ABIERTA", DateTime.ParseExact("05/01/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 11, 24, 35, 54 }), null, null, DateTime.MinValue, false);
            AltaSubasta("Vuelta ciclista", "ABIERTA", DateTime.ParseExact("06/01/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 27, 33, 39 }), null, null, DateTime.MinValue, new List<Oferta>());
            AltaSubasta("Set camping", "ABIERTA", DateTime.ParseExact("21/07/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 7, 34 ,36 }), null, null, DateTime.MinValue, new List<Oferta>());
            AltaVenta("Juego gimnasio", "ABIERTA", DateTime.ParseExact("13/12/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 14, 15, 25, 26, 28, 38 }), null, null, DateTime.MinValue, false);
            AltaVenta("Caminata en el bosque", "ABIERTA", DateTime.ParseExact("12/02/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 1, 3, 4, 5 }), null, null, DateTime.MinValue, false);
            AltaVenta("Paseo en bicicleta", "ABIERTA", DateTime.ParseExact("15/03/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 6, 8, 9, 10 }), null, null, DateTime.MinValue, false);
            AltaVenta("Clase de yoga", "ABIERTA", DateTime.ParseExact("22/04/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 12, 13, 16, 18, 20 }), null, null, DateTime.MinValue, false);
            AltaVenta("Día de spa", "ABIERTA", DateTime.ParseExact("30/05/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 21, 22, 23, 29 }), null, null, DateTime.MinValue, false);
            AltaVenta("Concierto al aire libre", "ABIERTA", DateTime.ParseExact("01/08/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 30, 31, 32, 34, 37 }), null, null, DateTime.MinValue, false);
            AltaVenta("Cata de vinos", "ABIERTA", DateTime.ParseExact("10/09/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 40, 41, 42 }), null, null, DateTime.MinValue, false);
            AltaVenta("Taller de pintura", "ABIERTA", DateTime.ParseExact("15/10/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 43, 44, 45, 46 }), null, null, DateTime.MinValue, false);
            AltaVenta("Excursión a la montaña", "ABIERTA", DateTime.ParseExact("25/11/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 47, 48, 49 }), null, null, DateTime.MinValue, false);
            AltaSubasta("Torneo de ajedrez", "ABIERTA", DateTime.ParseExact("12/03/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 50, 51, 52 }), null, null, DateTime.MinValue, new List<Oferta>());
            AltaSubasta("Subasta de arte", "ABIERTA", DateTime.ParseExact("20/04/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 51, 53, 54 }), null, null, DateTime.MinValue, new List<Oferta>());
            AltaSubasta("Rally de coches", "ABIERTA", DateTime.ParseExact("01/06/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 36, 37, 38 }), null, null, DateTime.MinValue, new List<Oferta>());
            AltaSubasta("Subasta de antigüedades", "ABIERTA", DateTime.ParseExact("15/07/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 29, 20, 21 }), null, null, DateTime.MinValue, new List<Oferta>());
            AltaSubasta("Concurso de cocina", "ABIERTA", DateTime.ParseExact("05/08/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 42, 43, 44 }), null, null, DateTime.MinValue, new List<Oferta>());
            AltaSubasta("Maratón de lectura", "ABIERTA", DateTime.ParseExact("12/09/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 45, 46, 47 }), null, null, DateTime.MinValue, new List<Oferta>());
            AltaSubasta("Competencia de fotografía", "ABIERTA", DateTime.ParseExact("30/10/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 18, 19, 20 }), null, null, DateTime.MinValue, new List<Oferta>());
            AltaSubasta("Fiesta de disfraces", "ABIERTA", DateTime.ParseExact("15/11/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 21, 22, 23 }), null, null, DateTime.MinValue, new List<Oferta>());
        }
        #endregion
        #region Usuario
        private void PrecargaUsuario()
        {
            AltaAdministrador("Valentin", "Latorre", "ValentinLatorre@Gmail.com", "Valentin1234");
            AltaAdministrador("Agustin", "Butrico", "AgustinButrico@gmail.com", "Agustin1234");
            AltaCliente("Juan", "Peres", "Juanperes@hmail.com", "Juan1234", 2000);
            AltaCliente("Esteban", "Lopez", "EstebanLopez@hmail.com", "5566AS43", 2000);
            AltaCliente("Carlos", "Medina", "CarlosMedina@hmail.com", "Medina1234", 4500);
            AltaCliente("Mariano", "Morales", "MarianoMorales@hmail.com", "Mariano2", 5000);
            AltaCliente("Estela", "Rosales", "EstelaRosales@hmail.com", "Rosalia46", 300);
            AltaCliente("Marcos", "Sauce", "MarcosSauce@hmail.com", "Sauce311", 30000);
            AltaCliente("Lucia", "Gomez", "LuciaGomezs@hmail.com", "Lucia1990", 7200);
            AltaCliente("Rodrigo", "Barrios", "RodrigoBarrios@hmail.com", "RodrigoBarrios12", 900);
            AltaCliente("Pepe", "Argento", "PepeArgento@gmail.com", "PepeArgento1113", 3300);
            AltaCliente("Felipe", "Castañeda", "FelipeCastañeda@gmail.com", "FeliCastañeda032", 3300);
        }
        #endregion
        #region Oferta
        private void PrecargaOferta()
        {
            AltaOferta(ObtenerUsuarioPorId(3, true, false), ObtenerPublicacionPorId(2, false, true), 100, DateTime.ParseExact("24/07/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(6, true, false), ObtenerPublicacionPorId(1, false, true), 1500, DateTime.ParseExact("24/07/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(4, true, false), ObtenerPublicacionPorId(1, false, true), 3400, DateTime.ParseExact("24/07/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(8, true, false), ObtenerPublicacionPorId(1, false, true), 3500, DateTime.ParseExact("24/07/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(3, true, false), ObtenerPublicacionPorId(1, false, true), 20000, DateTime.ParseExact("24/07/2024", "dd/MM/yyyy", null));
        }
        #endregion
        #endregion
    }
}
