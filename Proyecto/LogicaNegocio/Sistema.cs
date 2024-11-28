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
        private static Sistema? _instancia; // Instancia única
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
        #endregion
        #region Usuario
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
        public void AltaVenta(string nombre, string estado, DateTime fecha, List<Articulo> articulos, Cliente? cliente, DateTime fechaFin, bool ofertaRelampago)
        {
            Venta nuevaVenta = new Venta(nombre, estado, fecha, articulos, cliente, fechaFin, ofertaRelampago);
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
                throw new ArgumentException("Ya existe un usuario registrado con el nombre y apellido proporcionados");
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
                throw new ArgumentException("Ya existe un usuario registrado con el nombre y apellido proporcionados");
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
        /// Las funciones de transacción se encargan de cobrar y hacer la lógica de compra
        /// </summary>
        #region Transacciones
        public void CompraVenta(Cliente? cliente, Venta? venta)
        {
            // Cambia de estado la venta, registra el Cliente que la compró y la fecha de fin
            venta.Estado = "CERRADA";
            venta.Cliente = cliente;
            venta.FechaFin = DateTime.Now;

            // Cobra el valor de la venta al cliente
            decimal precioVenta = ConsultarPrecioVenta(venta, venta.Articulos);
            cliente.Saldo -= precioVenta;
        }
        public void CompraSubasta(Administrador? administrador, Subasta? subasta)
        {
            // Variable para determinar el cliente con el mayor monto que puede pagar
            bool fueCobrada = false;
            
            // Cobra el valor de la venta al cliente con la oferta más alta y saldo disponible
            for (int i = subasta.Ofertas.Count - 1; i >= 0 || !fueCobrada; i--)
            {
                Cliente? clienteActual = subasta.Ofertas[i].Usuario as Cliente;

                if (clienteActual.Saldo >= subasta.Ofertas[i].Monto)
                {
                    // Cambia de estado la subasta y registra el cliente que la ganó
                    subasta.Estado = "CERRADA";
                    subasta.Cliente = clienteActual;
                    // Registra el Administrador que cerro la subasta y la fecha fin
                    subasta.Administrador = administrador;
                    subasta.FechaFin = DateTime.Now;

                    clienteActual.Saldo -= subasta.Ofertas[i].Monto;

                    fueCobrada = true;
                }
            }
        }
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
            AltaVenta("Verano en la playa", "ABIERTA", DateTime.ParseExact("05/01/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 11, 24, 35, 54 }), null, DateTime.MinValue, false);
            AltaVenta("Juego gimnasio", "ABIERTA", DateTime.ParseExact("13/12/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 14, 15, 25, 26, 28, 38 }), null, DateTime.MinValue, false);
            AltaVenta("Caminata en el bosque", "ABIERTA", DateTime.ParseExact("12/02/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 1, 3, 4, 5 }), null, DateTime.MinValue, false);
            AltaVenta("Paseo en bicicleta", "ABIERTA", DateTime.ParseExact("15/03/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 6, 8, 9, 10 }), null, DateTime.MinValue, false);
            AltaVenta("Clase de yoga", "ABIERTA", DateTime.ParseExact("22/04/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 12, 13, 16, 18, 20 }), null, DateTime.MinValue, false);
            AltaVenta("Día de spa", "ABIERTA", DateTime.ParseExact("30/05/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 21, 22, 23, 29 }), null, DateTime.MinValue, true);
            AltaVenta("Concierto al aire libre", "ABIERTA", DateTime.ParseExact("01/08/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 30, 31, 32, 34, 37 }), null, DateTime.MinValue, false);
            AltaVenta("Cata de vinos", "ABIERTA", DateTime.ParseExact("10/09/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 40, 41, 42 }), null, DateTime.MinValue, false);
            AltaVenta("Taller de pintura", "CERRADA", DateTime.ParseExact("15/10/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 43, 44, 45, 46 }), ObtenerUsuarioPorId(3, true, false) as Cliente, DateTime.ParseExact("05/11/2024", "dd/MM/yyyy", null), false);
            AltaVenta("Excursión a la montaña", "CERRADA", DateTime.ParseExact("25/11/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 47, 48, 49 }), ObtenerUsuarioPorId(3, true, false) as Cliente, DateTime.ParseExact("26/11/2024", "dd/MM/yyyy", null), false);
            AltaSubasta("Vuelta ciclista", "CERRADA", DateTime.ParseExact("06/01/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 27, 33, 39 }), ObtenerUsuarioPorId(8, true, false) as Cliente, ObtenerUsuarioPorId(1, false, true) as Administrador, DateTime.ParseExact("30/07/2024", "dd/MM/yyyy", null), new List<Oferta>());
            AltaSubasta("Set camping", "ABIERTA", DateTime.ParseExact("21/07/2024", "dd/MM/yyyy", null), ObtenerArticuloPorId(new List<int> { 7, 34 ,36 }), null, null, DateTime.MinValue, new List<Oferta>());
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
            AltaCliente("Juan", "Peres", "Juanperes@hmail.com", "Juan1234", 5600);
            AltaCliente("Esteban", "Lopez", "EstebanLopez@hmail.com", "5566AS43", 27000);
            AltaCliente("Carlos", "Medina", "CarlosMedina@hmail.com", "Medina1234", 7500);
            AltaCliente("Mariano", "Morales", "MarianoMorales@hmail.com", "Mariano2", 5000);
            AltaCliente("Estela", "Rosales", "EstelaRosales@hmail.com", "Rosalia46", 1700);
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
            AltaOferta(ObtenerUsuarioPorId(3, true, false), ObtenerPublicacionPorId(10, false, true), 120, DateTime.ParseExact("06/01/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(6, true, false), ObtenerPublicacionPorId(10, false, true), 1500, DateTime.ParseExact("24/07/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(4, true, false), ObtenerPublicacionPorId(10, false, true), 3400, DateTime.ParseExact("24/07/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(8, true, false), ObtenerPublicacionPorId(10, false, true), 3500, DateTime.ParseExact("24/07/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(8, true, false), ObtenerPublicacionPorId(11, false, true), 100, DateTime.ParseExact("24/07/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(5, true, false), ObtenerPublicacionPorId(11, false, true), 500, DateTime.ParseExact("21/07/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(3, true, false), ObtenerPublicacionPorId(11, false, true), 20000, DateTime.ParseExact("24/07/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(6, true, false), ObtenerPublicacionPorId(12, false, true), 200, DateTime.ParseExact("12/03/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(8, true, false), ObtenerPublicacionPorId(12, false, true), 400, DateTime.ParseExact("20/04/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(7, true, false), ObtenerPublicacionPorId(12, false, true), 700, DateTime.ParseExact("01/06/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(10, true, false), ObtenerPublicacionPorId(15, false, true), 300, DateTime.ParseExact("05/08/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(9, true, false), ObtenerPublicacionPorId(15, false, true), 600, DateTime.ParseExact("15/07/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(11, true, false), ObtenerPublicacionPorId(17, false, true), 450, DateTime.ParseExact("12/09/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(3, true, false), ObtenerPublicacionPorId(17, false, true), 1550, DateTime.ParseExact("30/10/2024", "dd/MM/yyyy", null));
            AltaOferta(ObtenerUsuarioPorId(4, true, false), ObtenerPublicacionPorId(17, false, true), 1600, DateTime.ParseExact("30/10/2024", "dd/MM/yyyy", null));
        }
        #endregion
        #endregion
    }
}
