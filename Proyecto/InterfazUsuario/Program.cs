using LogicaNegocio;
using System;

namespace InterfazUsuario
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MenuTipoUsuario();
        }

        // Crear una instancia de la clase Sistema
        private static Sistema miSistema = new Sistema();

        /// <summary>
        /// La estructura modular de los menus permite navegarlos de distinta
        /// forma dependiendo del rol que tengas asignado (el primer menu lo determina).
        /// Cada vez que se selecciona una opción se ejecuta una funcion o se redirige a otro menu.
        /// Cada vez que se ejecuta un menu se borra el contenido en pantalla.
        /// La función MostrarOpcionesMenu(string[] opciones) permite imprimir las opciones de los menus en pantalla.
        /// </summary>
        #region Menu
        #region Utilidades
        static void MostrarOpcionesMenu(string[] opciones)
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine(opciones[0]);
            for (int i = 1; i < opciones.Length; i++)
            {
                Console.WriteLine(opciones[i]);
            }
            Console.WriteLine("-------------------------------------");
        }
        static void MostrarOpcionesMenuPorTipoUsuario(string tipoUsuario, string[] opcionesCliente, string[] opcionesAdministrador, string[] opcionesTester)
        {
            switch (tipoUsuario)
            {
                case "CLIENTE":
                    MostrarOpcionesMenu(opcionesCliente);
                    break;
                case "ADMINISTRADOR":
                    MostrarOpcionesMenu(opcionesAdministrador);
                    break;
                case "TESTER":
                    MostrarOpcionesMenu(opcionesTester);
                    break;
            }
        }
        static void VolverAlMenu()
        {
            Console.WriteLine("Precione Intro para volver al menu");
            Console.ReadLine();
        }
        /// <summary>
        /// Implementa el bloque try catch a los menus
        /// idMenu = 0 == MenuArticulo
        /// idMenu = 1 == MenuBuscarArticulo
        /// idMenu = 2 == MenuPublicacion
        /// idMenu = 3 == MenuMostrarPublicacion
        /// idMenu = 4 == MenuBuscarPublicacion
        /// idMenu = 5 == MenuOfetaSubasta
        /// idMenu = 6 == MenuAltaPublicacion
        /// idMenu = 7 == MenuUsuario
        /// idMenu = 8 == MenuMostrarUsuario
        /// idMenu = 9 == MenuBuscarUsuario
        /// idMenu = 10 == MenuAltaUsuario
        /// </summary>
        static bool ValidacionMenu(int idMenu, string tipoUsuario)
        {
            try
            {
                switch (idMenu)
                {
                    case 0:
                        MenuArticulo(tipoUsuario);
                        break;
                    case 1:
                        MenuBuscarArticulo(tipoUsuario);
                        break;
                    case 2:
                        MenuPublicacion(tipoUsuario);
                        break;
                    case 3:
                        MenuMostrarPublicacion(tipoUsuario);
                        break;
                    case 4:
                        MenuBuscarPublicacion(tipoUsuario);
                        break;
                    case 5:
                        MenuOfetaSubasta(tipoUsuario);
                        break;
                    case 6:
                        MenuAltaPublicacion(tipoUsuario);
                        break;
                    case 7:
                        MenuUsuario(tipoUsuario);
                        break;
                    case 8:
                        MenuMostrarUsuario(tipoUsuario);
                        break;
                    case 9:
                        MenuBuscarUsuario(tipoUsuario);
                        break;
                    case 10:
                        MenuAltaUsuario(tipoUsuario);
                        break;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error de operación: {ex.Message}");
                VolverAlMenu();
                return false;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Falta un argumento obligatorio: {ex.Message}");
                VolverAlMenu();
                return false;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Argumento inválido: {ex.Message}");
                VolverAlMenu();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                VolverAlMenu();
                return false;
            }
            return true;
        }
        #endregion
        #region Principal
        static void MenuTipoUsuario()
        {
            bool valido = false;
            string[] opciones = new string[] { "Menu selección tipo de usuario", "S. Salir del sistema", "1. Usuario", "2. Administrador", "3. Tester" };
            string tipoUsuario;

            while (!valido)
            {
                Console.Clear();
                MostrarOpcionesMenu(opciones); // Imprime las opciones del array opciones
                char.TryParse(Console.ReadLine(), out char opcionSeleccionada);

                switch (opcionSeleccionada)
                {
                    case 'S':
                        valido = true;
                        break;
                    case 's':
                        valido = true;
                        break;
                    case '1':
                        valido = true;
                        tipoUsuario = "CLIENTE";
                        Menu(tipoUsuario);
                        break;
                    case '2':
                        valido = true;
                        tipoUsuario = "ADMINISTRADOR";
                        Menu(tipoUsuario);
                        break;
                    case '3':
                        valido = true;
                        tipoUsuario = "TESTER";
                        Menu(tipoUsuario);
                        break;
                }
            }
        }
        static void Menu(string tipoUsuario)
        {
            bool valido = false;
            string[] opciones = new string[] { "Menu", "0. ...", "1. Artículos", "2. Publicaciones", "3. Usuarios" };

            while (!valido)
            {
                if (tipoUsuario == "CLIENTE" || tipoUsuario == "ADMINISTRADOR" || tipoUsuario == "TESTER")
                {
                    Console.Clear();
                    MostrarOpcionesMenu(opciones); // Imprime las opciones del array opciones
                    char.TryParse(Console.ReadLine(), out char opcionSeleccionada);

                    switch (opcionSeleccionada)
                    {
                        case '0':
                            valido = true;
                            MenuTipoUsuario();
                            break;
                        case '1':
                            valido = ValidacionMenu(0, tipoUsuario);
                            break;
                        case '2':
                            valido = ValidacionMenu(2, tipoUsuario);
                            break;
                        case '3':
                            valido = ValidacionMenu(7, tipoUsuario);
                            break;
                    }
                }
            }
        }
        #endregion
        #region Articulo
        static void MenuArticulo(string tipoUsuario)
        {
            bool valido = false;
            string[] opcionesCliente = new string[] { "Menu Articulo", "0. ...", "1. Mostrar catálogo", "2. Buscar" };
            string[] opcionesAdministrador = new string[] { "Menu Articulo", "0. ...", "1. Mostrar catálogo", "2. Buscar", "3. Dar de alta articulo" };

            while (!valido)
            {
                Console.Clear();
                MostrarOpcionesMenuPorTipoUsuario(tipoUsuario, opcionesCliente, opcionesAdministrador, opcionesAdministrador); // Imprime las opciones del menu por tipo de usuario
                char.TryParse(Console.ReadLine(), out char opcionSeleccionada);

                valido = ProcesamientoOpcionArticulo(tipoUsuario, opcionSeleccionada, valido);
            }
        }
        static void MenuBuscarArticulo(string tipoUsuario)
        {
            bool valido = false;
            string[] opciones = new string[] { "Menu Buscar", "0. ...", "1. Buscar artículo por ID", "2. Buscar artículo por Nombre", "3. Buscar artículo por categoría" };

            while (!valido)
            {
                Console.Clear();                
                MostrarOpcionesMenu(opciones); // Imprime las opciones del menu
                char.TryParse(Console.ReadLine(), out char opcionSeleccionada);

                valido = OpcionBuscarArticulo(tipoUsuario, opcionSeleccionada, valido);
            }
        }
        #endregion
        #region Publicacion
        static void MenuPublicacion(string tipoUsuario)
        {
            bool valido = false;
            string[] opcionesCliente = new string[] { "Menu Publicacion", "0. ...", "1. Mostrar", "2. Buscar", "3. Ofertar" };
            string[] opcionesAdministrador = new string[] { "Menu Publicacion", "0. ...", "1. Mostrar", "2. Buscar", "3. Ofertar", "4. Dar de alta" };

            while (!valido)
            {
                Console.Clear();
                MostrarOpcionesMenuPorTipoUsuario(tipoUsuario, opcionesCliente, opcionesAdministrador, opcionesAdministrador); // Imprime las opciones del menu por tipo de usuario
                char.TryParse(Console.ReadLine(), out char opcionSeleccionada);

                valido = ProcesamientoOpcionPublicacion(tipoUsuario, opcionSeleccionada, valido);
            }
        }
        static void MenuMostrarPublicacion(string tipoUsuario)
        {
            bool valido = false;
            string[] opciones = new string[] { "Menu Mostrar", "0. ...", "1. Mostrar todas las publicaciones", "2. Mostrar todas las ventas", "3. Mostrar todas las subastas" };

            while (!valido)
            {
                Console.Clear();
                MostrarOpcionesMenu(opciones); // Imprime las opciones del menu
                char.TryParse(Console.ReadLine(), out char opcionSeleccionada);

                valido = OpcionMostrarPublicacion(tipoUsuario, opcionSeleccionada, valido);
            }
        }
        static void MenuBuscarPublicacion(string tipoUsuario)
        {
            bool valido = false;
            string[] opciones = new string[] { "Menu Busqueda", "0. ...", "1. Buscar publicaciones por ID", "2. Buscar publicaciones por Nombre", "3. Buscar publicación por fecha" };

            while (!valido)
            {
                Console.Clear();
                MostrarOpcionesMenu(opciones); // Imprime las opciones del menu
                char.TryParse(Console.ReadLine(), out char opcionSeleccionada);

                valido = OpcionBuscarPublicacion(tipoUsuario, opcionSeleccionada, valido);
            }
        }
        static void MenuOfetaSubasta(string tipoUsuario)
        {
            bool valido = false;
            string[] opciones = new string[] { "Menu Ofertar", "0. ...", "1. Ofertar con tú ID", "2. Ofertar con tú Nombre" };

            while (!valido)
            {
                Console.Clear();
                MostrarOpcionesMenu(opciones); // Imprime las opciones del menu
                char.TryParse(Console.ReadLine(), out char opcionSeleccionada);

                valido = OpcionOfertaSubasta(tipoUsuario, opcionSeleccionada, valido);
            }
        }
        static void MenuAltaPublicacion(string tipoUsuario)
        {
            bool valido = false;
            string[] opcionesCliente = new string[] { "" };
            string[] opcionesAdministrador = new string[] { "Menu Alta", "0. ...", "1. Dar de alta venta", "2. Dar de alta subasta" };
            string[] opcionesTester = new string[] { "Menu Alta", "0. ...", "1. Dar de alta publicacion", "2. Dar de alta venta", "3. Dar de alta subasta" };

            while (!valido)
            {
                Console.Clear();
                MostrarOpcionesMenuPorTipoUsuario(tipoUsuario, opcionesCliente, opcionesAdministrador, opcionesTester); // Imprime las opciones del menu por tipo de usuario
                char.TryParse(Console.ReadLine(), out char opcionSeleccionada);

                valido = ProcesamientoOpcionAltaPublicacion(tipoUsuario, opcionSeleccionada, valido);
            }
        }
        #endregion
        #region Usuario
        static void MenuUsuario(string tipoUsuario)
        {
            bool valido = false;
            string[] opcionesCliente = new string[] { "Menu Usuarios", "0. ...", "1. Mostrar todos los clientes", "2. Buscar" };
            string[] opcionesAdministrador = new string[] { "Menu Usuarios", "0. ...", "1. Mostrar", "2. Buscar", "3. Dar de alta" };

            while (!valido)
            {
                Console.Clear();
                MostrarOpcionesMenuPorTipoUsuario(tipoUsuario, opcionesCliente, opcionesAdministrador, opcionesAdministrador); // Imprime las opciones del menu por tipo de usuario
                char.TryParse(Console.ReadLine(), out char opcionSeleccionada);

                valido = ProcesamientoOpcionUsuario(tipoUsuario, opcionSeleccionada, valido);
            }
        }
        static void MenuMostrarUsuario(string tipoUsuario)
        {
            bool valido = false;
            string[] opciones = new string[] { "Menu Mostrar", "0. ...", "1. Mostrar todos los usuarios", "2. Mostrar todos los clientes", "3. Mostrar todos los administradores" };

            while (!valido)
            {
                Console.Clear();
                MostrarOpcionesMenu(opciones); // Imprime las opciones del menu
                char.TryParse(Console.ReadLine(), out char opcionSeleccionada);

                valido = OpcionMostrarUsuario(tipoUsuario, opcionSeleccionada, valido);
            }
        }
        static void MenuBuscarUsuario(string tipoUsuario)
        {
            bool valido = false;
            string[] opciones = new string[] { "Menu Busqueda", "0. ...", "1. Buscar usuario por ID", "2. Buscar usuario por Nombre" };

            while (!valido)
            {
                Console.Clear();
                MostrarOpcionesMenu(opciones); // Imprime las opciones del menu
                char.TryParse(Console.ReadLine(), out char opcionSeleccionada);

                valido = OpcionBuscarUsuario(tipoUsuario, opcionSeleccionada, valido);
            }
        }
        static void MenuAltaUsuario(string tipoUsuario)
        {
            bool valido = false;
            string[] opcionesCliente = new string[] { "" };
            string[] opcionesAdministrador = new string[] { "Menu Alta", "0. ...", "1. Dar de alta cliente", "2. Dar de alta administrador" };
            string[] opcionesTester = new string[] { "Menu Alta", "0. ...", "1. Dar de alta usuario", "2. Dar de alta cliente", "3. Dar de alta administrador" };

            while (!valido)
            {
                Console.Clear();
                MostrarOpcionesMenuPorTipoUsuario(tipoUsuario, opcionesCliente, opcionesAdministrador, opcionesTester); // Imprime las opciones del menu por tipo de usuario
                char.TryParse(Console.ReadLine(), out char opcionSeleccionada);

                valido = ProcesamientoOpcionAltaUsuario(tipoUsuario, opcionSeleccionada, valido);
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Las opciones del menu trabajan con la variable opcionSeleccionada,
        /// esta variable es modificada si el menu es diferente para algún tipo de usuario
        /// ya que el indice que se muestra para cada opción en el menu cambia.
        /// Por ejemplo, si cliente tiene 2 opciones disponibles en el menu y administrador
        /// tiene 3 opciones disponibles, con una función intermedia se modifica el input del cliente
        /// si este es == 3 y se remplaza con 99.
        /// Esto permite evitar que el usuario acceda a una opción que no tiene disponible.
        /// </summary>
        #region Opciones del menu
        #region Articulo
        static bool ProcesamientoOpcionArticulo(string tipoUsuario, int opcionSelecionada, bool valido)
        {
            if (tipoUsuario == "CLIENTE")
            {
                if (opcionSelecionada == '3')
                {
                    opcionSelecionada = 'F';
                }
            }

            return OpcionArticulo(tipoUsuario, opcionSelecionada, valido);
        }
        static bool OpcionArticulo(string tipoUsuario, int opcionSeleccionada, bool valido)
        {

            switch (opcionSeleccionada)
            {
                case '0':
                    valido = true;
                    Menu(tipoUsuario);
                    break;
                case '1':
                    Console.WriteLine(new string('\n', 40));
                    Console.Clear();
                    ImprimirArticulo(miSistema.ObtenerArticulos(), true);
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(0, tipoUsuario);
                    break;
                case '2':
                    valido = ValidacionMenu(1, tipoUsuario);
                    break;
                case '3':
                    AltaArticulo();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(0, tipoUsuario);
                    break;
            }
            return valido;
        }
        static bool OpcionBuscarArticulo(string tipoUsuario, int opcionSeleccionada, bool valido)
        {
            switch (opcionSeleccionada)
            {
                case '0':
                    valido = ValidacionMenu(0, tipoUsuario);
                    break;
                case '1':
                    ObtenerArticuloPorId();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(1, tipoUsuario);
                    break;
                case '2':
                    ObtenerArticuloPorNombre();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(1, tipoUsuario);
                    break;
                case '3':
                    ObtenerArticuloPorCategoria();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(1, tipoUsuario);
                    break;
            }
            return valido;
        }
        #endregion
        #region Publicacion
        static bool ProcesamientoOpcionPublicacion(string tipoUsuario, int opcionSelecionada, bool valido)
        {
            if (tipoUsuario == "CLIENTE")
            {
                if (opcionSelecionada == '4')
                {
                    opcionSelecionada = 'F';
                }
            }

            return OpcionPublicacion(tipoUsuario, opcionSelecionada, valido);
        }
        static bool OpcionPublicacion(string tipoUsuario, int opcionSelecionada, bool valido)
        {
            // Ejecución de las opciones del menu por tipo de usuario
            switch (opcionSelecionada)
            {
                case '0':
                    valido = true;
                    Menu(tipoUsuario);
                    break;
                case '1':
                    valido = ValidacionMenu(3, tipoUsuario);
                    break;
                case '2':
                    valido = ValidacionMenu(4, tipoUsuario);
                    break;
                case '3':
                    valido = ValidacionMenu(5, tipoUsuario);
                    break;
                case '4':
                    valido = ValidacionMenu(6, tipoUsuario);
                    break;
            }
            return valido;
        }
        static bool OpcionMostrarPublicacion(string tipoUsuario, int opcionSeleccionada, bool valido)
        {
            switch (opcionSeleccionada)
            {
                case '0':
                    valido = ValidacionMenu(2, tipoUsuario);
                    break;
                case '1':
                    Console.WriteLine(new string('\n', 40));
                    Console.Clear();
                    ImprimirPublicacion(miSistema.ObtenerPublicaciones(false, false), true);
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(3, tipoUsuario);
                    break;
                case '2':
                    Console.WriteLine(new string('\n', 40));
                    Console.Clear();
                    ImprimirPublicacion(miSistema.ObtenerPublicaciones(true, false), true);
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(3, tipoUsuario);
                    break;
                case '3':
                    Console.WriteLine(new string('\n', 40));
                    Console.Clear();
                    ImprimirPublicacion(miSistema.ObtenerPublicaciones(false, true), true);
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(3, tipoUsuario);
                    break;
            }
            return valido;
        }
        static bool OpcionBuscarPublicacion(string tipoUsuario, int opcionSeleccionada, bool valido)
        {
            switch (opcionSeleccionada)
            {
                case '0':
                    valido = ValidacionMenu(2, tipoUsuario);
                    break;
                case '1':
                    ObtenerPublicacionPorId();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(4, tipoUsuario);
                    break;
                case '2':
                    ObtenerPublicacionPorNombre();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(4, tipoUsuario);
                    break;
                case '3':
                    ObtenerPublicacionPorFecha();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(4, tipoUsuario);
                    break;
            }
            return valido;
        }
        static bool OpcionOfertaSubasta(string tipoUsuario, int opcionSeleccionada, bool valido)
        {
            switch (opcionSeleccionada)
            {
                case '0':
                    valido = ValidacionMenu(2, tipoUsuario);
                    break;
                case '1':
                    AltaOfertaPorId();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(5, tipoUsuario);
                    break;
                case '2':
                    AltaOfertaPorNombre();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(5, tipoUsuario);
                    break;
            }
            return valido;
        }
        static bool ProcesamientoOpcionAltaPublicacion(string tipoUsuario, int opcionSelecionada, bool valido)
        {
            if (tipoUsuario == "ADMINISTRADOR")
            {
                if (opcionSelecionada == '3')
                {
                    opcionSelecionada = 'F';
                }
                if (opcionSelecionada == '2')
                {
                    opcionSelecionada = '3';
                }
                if (opcionSelecionada == '1')
                {
                    opcionSelecionada = '2';
                }
            }

            return OpcionAltaPublicacion(tipoUsuario, opcionSelecionada, valido);
        }
        static bool OpcionAltaPublicacion(string tipoUsuario, int opcionSeleccionada, bool valido)
        {
            switch (opcionSeleccionada)
            {
                case '0':
                    valido = ValidacionMenu(2, tipoUsuario);
                    break;
                case '1':
                    AltaPublicacion();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(6, tipoUsuario);
                    break;
                case '2':
                    AltaVenta();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(6, tipoUsuario);
                    break;
                case '3':
                    AltaSubasta();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(6, tipoUsuario);
                    break;
            }
            return valido;
        }
        #endregion
        #region Usuario
        static bool ProcesamientoOpcionUsuario(string tipoUsuario, int opcionSelecionada, bool valido)
        {
            if (tipoUsuario == "CLIENTE")
            {
                if (opcionSelecionada == '3')
                {
                    opcionSelecionada = 'F';
                }
                if (opcionSelecionada == '4')
                {
                    opcionSelecionada = 'F';
                }
                if (opcionSelecionada == '1')
                {
                    opcionSelecionada = '4';
                }
            }
            else 
            {
                if (opcionSelecionada == '4')
                {
                    opcionSelecionada = 'F';
                }
            }

            return OpcionUsuario(tipoUsuario, opcionSelecionada, valido);
        }
        static bool OpcionUsuario(string tipoUsuario, int opcionSelecionada, bool valido)
        {
            switch (opcionSelecionada)
            {
                case '0':
                    valido = true;
                    Menu(tipoUsuario);
                    break;
                case '1':
                    valido = ValidacionMenu(8, tipoUsuario);
                    break;
                case '2':
                    valido = ValidacionMenu(9, tipoUsuario);
                    break;
                case '3':
                    valido = ValidacionMenu(10, tipoUsuario);
                    break;
                case '4':
                    Console.WriteLine(new string('\n', 40));
                    Console.Clear();
                    ImprimirUsuario(miSistema.ObtenerUsuarios(true, false));
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(7, tipoUsuario);
                    break;
            }
            return valido;
        }
        static bool OpcionMostrarUsuario(string tipoUsuario, int opcionSeleccionada, bool valido)
        {
            switch (opcionSeleccionada)
            {
                case '0':
                    valido = ValidacionMenu(7, tipoUsuario);
                    break;
                case '1':
                    Console.WriteLine(new string('\n', 40));
                    Console.Clear();
                    ImprimirUsuario(miSistema.ObtenerUsuarios(false, false));
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(8, tipoUsuario);
                    break;
                case '2':
                    Console.WriteLine(new string('\n', 40));
                    Console.Clear();
                    ImprimirUsuario(miSistema.ObtenerUsuarios(true, false));
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(8, tipoUsuario);
                    break;
                case '3':
                    Console.WriteLine(new string('\n', 40));
                    Console.Clear();
                    ImprimirUsuario(miSistema.ObtenerUsuarios(false, true));
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(8, tipoUsuario);
                    break;
            }
            return valido;
        }
        static bool OpcionBuscarUsuario(string tipoUsuario, int opcionSeleccionada, bool valido)
        {
            switch (opcionSeleccionada)
            {
                case '0':
                    valido = ValidacionMenu(7, tipoUsuario);
                    break;
                case '1':
                    ObtenerUsuarioPorId();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(9, tipoUsuario);
                    break;
                case '2':
                    ObtenerUsuarioPorNombre();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(9, tipoUsuario);
                    break;
            }
            return valido;
        }
        static bool ProcesamientoOpcionAltaUsuario(string tipoUsuario, int opcionSelecionada, bool valido)
        {
            if (tipoUsuario == "ADMINISTRADOR")
            {
                if (opcionSelecionada == '3')
                {
                    opcionSelecionada = 'F';
                }
                if (opcionSelecionada == '2')
                {
                    opcionSelecionada = '3';
                }
                if (opcionSelecionada == '1')
                {
                    opcionSelecionada = '2';
                }
            }

            return OpcionAltaUsuario(tipoUsuario, opcionSelecionada, valido);
        }
        static bool OpcionAltaUsuario(string tipoUsuario, int opcionSeleccionada, bool valido)
        {
            switch (opcionSeleccionada)
            {
                case '0':
                    valido = ValidacionMenu(7, tipoUsuario);
                    break;
                case '1':
                    AltaUsuario();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(10, tipoUsuario);
                    break;
                case '2':
                    AltaCliente();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(10, tipoUsuario);
                    break;
                case '3':
                    AltaAdministrador();
                    VolverAlMenu(); // Limpia la consola cuando el usuario preciona Intro
                    valido = ValidacionMenu(10, tipoUsuario);
                    break;
            }
            return valido;
        }
        #endregion
        #endregion

        /// <summary>
        /// Las funciones de impresión son las menores posibles para evitar diferencias en la impresion.
        /// Estas imprimen los datos basandose en listas de datos.
        /// Tambien tienen booleanos como margenesGrandes o vistaResumida que sirven para facilitar
        /// la lectura de los datos por parte del usuario al utilizar el programa
        /// </summary>
        #region Impresion de listas
        #region Articulo
        static void ImprimirArticulo(List<Articulo> articulos, bool margenesGrandes)
        {
            int margenes = 35;
            string menuAnidado = "  ";
            if (margenesGrandes)
            {
                margenes = 70;
                menuAnidado = "";
            }
            
            for (int i = 0; i < articulos.Count; i++)
            {
                Console.WriteLine(new string('-', margenes));
                // Mostramos los detalles del Artículo
                Console.WriteLine($"{menuAnidado}ID: {articulos[i].Id}");
                Console.WriteLine($"{menuAnidado}Nombre: {articulos[i].Nombre}");
                Console.WriteLine($"{menuAnidado}Precio: {articulos[i].Precio}");
                Console.WriteLine($"{menuAnidado}Categoría: {articulos[i].Categoria}");
            }
            Console.WriteLine(new string('-', margenes));
        }
        #endregion
        #region Publicacion
        static void ImprimirPublicacion(List<Publicacion> publicaciones, bool vistaResumida)
        {
            for (int i = 0; i < publicaciones.Count; i++)
            {
                Console.WriteLine(new string('-', 70));
                // Mostramos los detalles de las publicaciones
                Console.WriteLine($"ID: {publicaciones[i].Id}");
                Console.WriteLine($"Nombre: {publicaciones[i].Nombre}");
                Console.WriteLine($"Estado: {publicaciones[i].Estado}");
                Console.WriteLine($"Fecha de publicación: {publicaciones[i].Fecha}");
                Console.WriteLine($"Articulos: {publicaciones[i].Articulos.Count}");
                if (publicaciones[i].Articulos.Count > 0) // Si hay articulos imprimimos sus ids
                {
                    Console.WriteLine($"Id de los articulos asociados: {miSistema.ParseoArticulo(publicaciones[i].Articulos)}");
                    if (!vistaResumida) // Si no es vista resumida mostramos los datos de los articulos
                    {
                        ImprimirArticulo(publicaciones[i].Articulos, false); // Imprime los datos de los articulos asociados
                    }
                }
                if (publicaciones[i] is Venta venta)
                {
                    if (venta.OfertaRelampago)
                    {
                        Console.WriteLine($"Oferta relampago: Si");
                    }
                    else
                    {
                        Console.WriteLine($"Oferta relampago: No");
                    }
                    Console.WriteLine($"Precio venta {miSistema.ConsultarPrecioVenta(publicaciones[i], publicaciones[i].Articulos)}");

                }
                if (publicaciones[i] is Subasta subasta)
                {
                    Console.WriteLine($"Ofertas: {subasta.Ofertas.Count}");
                    if (subasta.Ofertas.Count > 0) // Si hay ofertas imprimimos sus ids
                    {
                        Console.WriteLine($"Id de las ofertas asociadas: {miSistema.ParseoOferta(subasta.Ofertas)}");
                        if (!vistaResumida) // Si no es vista resumida mostramos los datos de las ofertas
                        {
                            ImprimirOferta(subasta.Ofertas); // Imprime los datos de las ofertas asociadas
                        }
                    }
                }
                if (publicaciones[i].FechaFin != DateTime.MinValue)
                {
                    Console.WriteLine($"Cliente: {publicaciones[i].Cliente}");
                    Console.WriteLine($"Administrador: {publicaciones[i].Administrador}");
                    Console.WriteLine($"Fecha Fin: {publicaciones[i].FechaFin}");
                }
            }
            Console.WriteLine(new string('-', 70));
        }
        #endregion
        #region Usuario
        static void ImprimirUsuario(List<Usuario> usuarios)
        {
            if (usuarios.Count > 0)
            {
                for (int i = 0; i < usuarios.Count; i++)
                {
                    Console.WriteLine(new string('-', 70));
                    if (usuarios[i] is Administrador administrador)
                    {
                        // Mostramos los detalles del Administrador
                        Console.WriteLine($"ID: {administrador.Id}");
                        Console.WriteLine($"Nombre: {administrador.Nombre}");
                        Console.WriteLine($"Apellido: {administrador.Apellido}");
                        Console.WriteLine($"Email: {administrador.Email}");
                    }
                    if (usuarios[i] is Cliente cliente && miSistema.HayCliente())
                    {
                        // Mostramos los detalles del Usuario
                        Console.WriteLine($"ID: {cliente.Id}");
                        Console.WriteLine($"Nombre: {cliente.Nombre}");
                        Console.WriteLine($"Apellido: {cliente.Apellido}");
                        Console.WriteLine($"Email: {cliente.Email}");
                        Console.WriteLine($"Saldo: {cliente.Saldo}");
                    }
                }
                Console.WriteLine(new string('-', 70));
            }
            if (!miSistema.HayCliente())
            {
                Console.WriteLine("No existen clientes actualmente");
            }
        }
        #endregion
        #region Oferta
        static void ImprimirOferta(List<Oferta> ofertas)
        {
            string menuAnidado = "  ";
            for (int i = 0; i < ofertas.Count; i++)
            {
                Console.WriteLine(new string('-', 35));
                // Mostramos los detalles de las ofertas
                Console.WriteLine($"{menuAnidado}ID: {ofertas[i].Id}");
                if (ofertas[i].Usuario is Usuario usuario)
                {
                    Console.WriteLine($"{menuAnidado}Id usuario: {usuario.Id}");
                    Console.WriteLine($"{menuAnidado}Nombre usuario: {usuario.Nombre}");
                    Console.WriteLine($"{menuAnidado}Apellido usuario: {usuario.Apellido}");
                }
                Console.WriteLine($"{menuAnidado}Monto: {ofertas[i].Monto}");
                Console.WriteLine($"{menuAnidado}Fecha: {ofertas[i].Fecha}");
            }
            Console.WriteLine(new string('-', 35));
        }
        #endregion
        #endregion

        /// <summary>
        /// En las funciones de busqueda se solicitan datos simples como id o nombre
        /// pero tambien se habilita la posibilidad de buscar varios ids o nombres al mismo tiempo.
        /// Luego se imprimen en pantalla las busquedas realizadas utilizando estos datos con las 
        /// funciones de Obtener que se encuentran en Sistema
        /// </summary>
        #region Busqueda
        #region Articulo
        static void ObtenerArticuloPorId()
        {
            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Id de los articulos separados por ,:");
            string idsCrudos = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(idsCrudos))
            {
                Console.WriteLine("El id no puede ser vacío");
                return;
            }
            List<int> ids = miSistema.ParseoInt(idsCrudos); // Convierte el input del usuario en una lista de ids
            if (ids == null || ids.Count == 0)
            {
                Console.WriteLine("No se encontraron ids correspondientes a los ids proporcionados");
                return;
            }
            List<Articulo> articulos = miSistema.ObtenerArticuloPorId(ids);
            if (articulos == null || articulos.Count == 0)
            {
                Console.WriteLine("No se encontraron articulos correspondientes a los ids proporcionados");
                return;
            }

            Console.WriteLine(new string('\n', 40));
            Console.Clear();
            ImprimirArticulo(articulos, true);
        }
        static void ObtenerArticuloPorNombre()
        {
            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Nombre de los articulos separados por ,:");
            string nombresCrudos = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombresCrudos))
            {
                Console.WriteLine("El nombre no puede ser vacío");
                return;
            }
            List<string> nombres = miSistema.ParseoString(nombresCrudos); // Convierte el input del usuario en una lista de nombres
            if (nombres == null || nombres.Count == 0)
            {
                Console.WriteLine("No se encontraron nombres correspondientes a los nombres proporcionados");
                return;
            }
            List<Articulo> articulos = miSistema.ObtenerArticuloPorNombre(nombres);
            if (articulos == null || articulos.Count == 0)
            {
                Console.WriteLine("No se encontraron articulos correspondientes a los nombres proporcionados");
                return;
            }

            Console.WriteLine(new string('\n', 40));
            Console.Clear();
            ImprimirArticulo(articulos, true);
        }
        static void ObtenerArticuloPorCategoria()
        {
            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Categoria de los articulos separados por ,:");
            string categoriasCrudas = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(categoriasCrudas))
            {
                Console.WriteLine("La categoria no puede ser vacía");
                return;
            }
            List<string> categorias = miSistema.ParseoString(categoriasCrudas); // Convierte el input del usuario en una lista de nombres
            if (categorias == null || categorias.Count == 0)
            {
                Console.WriteLine("No se encontraron categorias correspondientes a las castegorias proporcionados");
                return;
            }
            List<Articulo> articulos = miSistema.ObtenerArticuloPorCategoria(categorias);
            if (articulos == null || articulos.Count == 0)
            {
                Console.WriteLine("No se encontraron articulos correspondientes a las categorias proporcionadas");
                return;
            }

            Console.WriteLine(new string('\n', 40));
            Console.Clear();
            ImprimirArticulo(articulos, true);
        }
        #endregion
        #region Publicacion
        static void ObtenerPublicacionPorId()
        {
            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Id de las publicaciones separadas por ,:");
            string idsCrudos = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(idsCrudos))
            {
                Console.WriteLine("El id no puede ser vacío");
                return;
            }
            List<int> ids = miSistema.ParseoInt(idsCrudos); // Convierte el input del usuario en una lista de ids
            if (ids == null || ids.Count == 0)
            {
                Console.WriteLine("No se encontraron ids correspondientes a los ids proporcionados");
                return;
            }
            List<Publicacion> publicaciones = miSistema.ObtenerPublicacionPorId(ids, false, false);
            if (publicaciones == null || publicaciones.Count == 0)
            {
                Console.WriteLine("No se encontraron publicaciones correspondientes a los ids proporcionados");
                return;
            }

            Console.WriteLine(new string('\n', 40));
            Console.Clear();
            ImprimirPublicacion(publicaciones, false);
        }
        static void ObtenerPublicacionPorNombre()
        {
            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Nombre de las publicaciones separadas por ,:");
            string nombresCrudos = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombresCrudos))
            {
                Console.WriteLine("El nombre no puede ser vacío");
                return;
            }
            List<string> nombres = miSistema.ParseoString(nombresCrudos); // Convierte el input del usuario en una lista de nombres
            if (nombres == null || nombres.Count == 0)
            {
                Console.WriteLine("No se encontraron nombres correspondientes a los nombres proporcionados");
                return;
            }
            List<Publicacion> publicaciones = miSistema.ObtenerPublicacionPorNombre(nombres, false, false);
            if (publicaciones == null || publicaciones.Count == 0)
            {
                Console.WriteLine("No se encontraron publicaciones correspondientes a los nombres proporcionados");
                return;
            }

            Console.WriteLine(new string('\n', 40));
            Console.Clear();
            ImprimirPublicacion(publicaciones, false);
        }
        static void ObtenerPublicacionPorFecha()
        {
            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Fecha inicio dd/MM/yyyy:");
            string fechaInicioCruda = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(fechaInicioCruda))
            {
                Console.WriteLine("La fecha de inicio no puede ser vacía");
                return;
            }
            if (!DateTime.TryParse(fechaInicioCruda, out DateTime fechaInicio))
            {
                Console.WriteLine("El formato de la fecha de inicio no es válido. Use dd/MM/yyyy.");
                return;
            }
            Console.WriteLine("Fecha fin dd/MM/yyyy:");
            string fechaFinCruda = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(fechaFinCruda))
            {
                Console.WriteLine("La fecha de fin no puede ser vacía");
                return;
            }
            if (!DateTime.TryParse(fechaFinCruda, out DateTime fechaFin))
            {
                Console.WriteLine("El formato de la fecha de inicio no es válido. Use dd/MM/yyyy.");
                return;
            }
            List<Publicacion> publicaciones = miSistema.ObtenerPublicacionPorFecha(fechaInicio, fechaFin, false, false);
            if (publicaciones == null || publicaciones.Count == 0)
            {
                Console.WriteLine("No se encontraron publicaciones correspondientes al intervalo proporcionado");
                return;
            }

            Console.WriteLine(new string('\n', 40));
            Console.Clear();
            ImprimirPublicacion(publicaciones, false);
        }
        #endregion
        #region Usuario
        static void ObtenerUsuarioPorId()
        {
            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Id de los usuarios separados por ,:");
            string idsCrudos = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(idsCrudos))
            {
                Console.WriteLine("El id no puede ser vacío");
                return;
            }
            List<int> ids = miSistema.ParseoInt(idsCrudos); // Convierte el input del usuario en una lista de ids
            if (ids == null || ids.Count == 0)
            {
                Console.WriteLine("No se encontraron ids correspondientes a los ids proporcionados");
                return;
            }
            List<Usuario> usuarios = miSistema.ObtenerUsuarioPorId(ids, false, false);
            if (usuarios == null || usuarios.Count == 0)
            {
                Console.WriteLine("No se encontraron usuarios correspondientes a los ids proporcionados");
                return;
            }

            Console.WriteLine(new string('\n', 40));
            Console.Clear();
            ImprimirUsuario(usuarios);
        }
        static void ObtenerUsuarioPorNombre()
        {
            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Nombre de los usuarios separados por ,:");
            string nombresCrudos = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombresCrudos))
            {
                Console.WriteLine("El nombre no puede ser vacío");
                return;
            }
            List<string> nombres = miSistema.ParseoString(nombresCrudos); // Convierte el input del usuario en una lista de nombres
            if (nombres == null || nombres.Count == 0)
            {
                Console.WriteLine("No se encontraron nombres correspondientes a los nombres proporcionados");
                return;
            }
            List<Usuario> usuarios = miSistema.ObtenerUsuarioPorNombre(nombres, false, false);
            if (usuarios == null || usuarios.Count == 0)
            {
                Console.WriteLine("No se encontraron usuarios correspondientes a los nombres proporcionados");
                return;
            }

            Console.WriteLine(new string('\n', 40));
            Console.Clear();
            ImprimirUsuario(usuarios);
        }
        #endregion
        #endregion

        /// <summary>
        /// En las funciones de alta se solicitan datos minimos para poder dar de alta lo solicitado.
        /// Algunos de los datos como la fecha de creación se asignan automaticamente (fecha.Now)
        /// </summary>
        #region Altas
        #region Articulo
        static void AltaArticulo()
        {
            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Ingrese los datos del artículo");
            Console.WriteLine("Nombre:");
            // ?? operador de coalescencia nula
            // Si es null devuelve el valor de la derecha
            // Si no es null devuelve el valor de la izquierda
            string nombre = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("El nombre no puede ser vacío");
                return;
            }
            Console.WriteLine("Precio:");
            decimal precio;
            if (!decimal.TryParse(Console.ReadLine(), out precio) || precio < 0)
            {
                Console.WriteLine("El peecio debe ser un número entero positivo");
                return;
            }
            Console.WriteLine("Categoría:");
            string categoria = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(categoria))
            {
                Console.WriteLine("La categoria no puede ser vacía");
                return;
            }

            miSistema.AltaArticulo(nombre, precio, categoria);
            Console.WriteLine("El articulo se ha creado exitosamente");
        }
        #endregion
        #region Publicacion
        static void AltaPublicacion()
        {
            // Valores por defecto
            string estado = "ABIERTA";
            DateTime fecha = DateTime.Now;
            Cliente? cliente = null;
            Administrador? administrador = null;
            DateTime fechaFin = DateTime.MinValue;

            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Ingrese los datos que desea asociar a la publicacion");
            Console.WriteLine("Nombre:");
            string nombre = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("El nombre no puede ser vacío");
                return;
            }
            Console.WriteLine("Id de los articulos separados por ,:");
            string idsCrudos = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(idsCrudos))
            {
                Console.WriteLine("El id no puede ser vacío");
                return;
            }
            List<int> ids = miSistema.ParseoInt(idsCrudos); // Convierte el input del usuario en una lista de ids
            if (ids == null || ids.Count == 0)
            {
                Console.WriteLine("No se proporcionaron IDs válidos");
                return;
            }
            List<Articulo> articulos = miSistema.ObtenerArticuloPorId(ids); // Obtiene la lista de publicaciones con los ids
            if (articulos == null || articulos.Count == 0)
            {
                Console.WriteLine("No se encontraron artículos correspondientes a los IDs proporcionados");
                return;
            }

            miSistema.AltaPublicacion(nombre, estado, fecha, articulos, cliente, administrador, fechaFin);
            Console.WriteLine("La publicación se ha creado exitosamente");
        }
        static void AltaVenta()
        {
            // Valores por defecto
            string estado = "ABIERTA";
            DateTime fecha = DateTime.Now;
            Cliente? cliente = null;
            Administrador? administrador = null;
            DateTime fechaFin = DateTime.MinValue;

            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Ingrese los datos que desea asociar a la venta");
            Console.WriteLine("Nombre:");
            string nombre = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("El nombre no puede ser vacío");
                return;
            }
            Console.WriteLine("Id de los articulos separados por ,:");
            string idsCrudos = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(idsCrudos))
            {
                Console.WriteLine("El id no puede ser vacío");
                return;
            }
            List<int> ids = miSistema.ParseoInt(idsCrudos); // Convierte el input del usuario en una lista de ids
            if (ids == null || ids.Count == 0)
            {
                Console.WriteLine("No se proporcionaron IDs válidos");
                return;
            }
            List<Articulo> articulos = miSistema.ObtenerArticuloPorId(ids); // Obtiene la lista de publicaciones con los ids
            if (articulos == null || articulos.Count == 0)
            {
                Console.WriteLine("No se encontraron artículos correspondientes a los IDs proporcionados");
                return;
            }
            Console.WriteLine("Es oferta relampago?\n1. Si\n2. No");
            int esOferta;
            if (!int.TryParse(Console.ReadLine(), out esOferta))
            {
                Console.WriteLine("Su respuesta debe ser 1 o 2");
                return;
            }
            bool ofertaRelampago = false;

            if ( esOferta == 1 )
            {
                ofertaRelampago = true;
            }

            miSistema.AltaVenta(nombre, estado, fecha, articulos, cliente, administrador, fechaFin, ofertaRelampago);
            Console.WriteLine("La venta se ha creado exitosamente");
        }
        static void AltaSubasta()
        {
            // Valores por defecto
            string estado = "ABIERTA";
            DateTime fecha = DateTime.Now;
            Cliente? cliente = null;
            Administrador? administrador = null;
            DateTime fechaFin = DateTime.MinValue;
            List<Oferta> ofertas = new List<Oferta>();

            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Ingrese los datos que desea asociar a la subasta");
            Console.WriteLine("Nombre:");
            string nombre = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("El nombre no puede ser vacío");
                return;
            }
            Console.WriteLine("Id de los articulos separados por ,:");
            string idsCrudos = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(idsCrudos))
            {
                Console.WriteLine("El id no puede ser vacío");
                return;
            }
            List<int> ids = miSistema.ParseoInt(idsCrudos); // Convierte el input del usuario en una lista de ids
            if (ids == null || ids.Count == 0)
            {
                Console.WriteLine("No se proporcionaron IDs válidos");
                return;
            }
            List<Articulo> articulos = miSistema.ObtenerArticuloPorId(ids); // Obtiene la lista de publicaciones con los ids
            if (articulos == null || articulos.Count == 0)
            {
                Console.WriteLine("No se encontraron artículos correspondientes a los IDs proporcionados");
                return;
            }

            miSistema.AltaSubasta(nombre, estado, fecha, articulos, cliente, administrador, fechaFin, ofertas);
            Console.WriteLine("La subasta se ha creado exitosamente");
        }
        #endregion
        #region Usuario
        static void AltaUsuario()
        {
            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Ingrese los datos que desea asociar al usuario");
            Console.WriteLine("Nombre:");
            string nombre = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("El nombre no puede ser vacío");
                return;
            }
            Console.WriteLine("Apellido:");
            string apellido = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(apellido))
            {
                Console.WriteLine("El apellido no puede ser vacío");
                return;
            }
            Console.WriteLine("Email:");
            string email = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("El email no puede ser vacío");
                return;
            }
            if (email.IndexOf('@') == -1)
            {
                Console.WriteLine("El email debe pertenecer a un domino (debe tener @)");
                return;
            }
            Console.WriteLine("Contraseña:");
            string contrasenia = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(contrasenia))
            {
                Console.WriteLine("La contraseña no puede ser vacía");
                return;
            }

            miSistema.AltaUsuario(nombre, apellido, email, contrasenia);
            Console.WriteLine("El usuario se ha creado exitosamente");
        }
        static void AltaCliente()
        {
            // Valores por defecto
            int saldo = 0;

            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Ingrese los datos que desea asociar al cliente");
            Console.WriteLine("Nombre:");
            string nombre = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("El nombre no puede ser vacío");
                return;
            }
            Console.WriteLine("Apellido:");
            string apellido = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(apellido))
            {
                Console.WriteLine("El apellido no puede ser vacío");
                return;
            }
            Console.WriteLine("Email:");
            string email = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("El email no puede ser vacío");
                return;
            }
            if (email.IndexOf('@') == -1)
            {
                Console.WriteLine("El email debe pertenecer a un domino (debe tener @)");
                return;
            }
            Console.WriteLine("Contraseña:");
            string contrasenia = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(contrasenia))
            {
                Console.WriteLine("La contraseña no puede ser vacía");
                return;
            }

            miSistema.AltaCliente(nombre, apellido, email, contrasenia, saldo);
            Console.WriteLine("El cliente se ha creado exitosamente");
        }
        static void AltaAdministrador()
        {
            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Ingrese los datos que desea asociar al administrador");
            Console.WriteLine("Nombre:");
            string nombre = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("El nombre no puede ser vacío");
                return;
            }
            Console.WriteLine("Apellido:");
            string apellido = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(apellido))
            {
                Console.WriteLine("El apellido no puede ser vacío");
                return;
            }
            Console.WriteLine("Email:");
            string email = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("El email no puede ser vacío");
                return;
            }
            if (email.IndexOf('@') == -1)
            {
                Console.WriteLine("El email debe pertenecer a un domino (debe tener @)");
                return;
            }
            Console.WriteLine("Contraseña:");
            string contrasenia = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(contrasenia))
            {
                Console.WriteLine("La contraseña no puede ser vacía");
                return;
            }

            miSistema.AltaAdministrador(nombre, apellido, email, contrasenia);
            Console.WriteLine("El administrador se ha creado exitosamente");
        }
        #endregion
        #region Oferta
        static void AltaOfertaPorId()
        {
            // Valores por defecto
            DateTime fecha = DateTime.Now;

            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Ingrese los datos que desea asociar a la oferta");
            Console.WriteLine("Id del comprador:");
            int idUsuario;
            if (!int.TryParse(Console.ReadLine(), out idUsuario) || idUsuario < 0)
            {
                Console.WriteLine("El ID del usuario debe ser un número entero positivo");
                return;
            }
            Usuario? usuario = miSistema.ObtenerUsuarioPorId(idUsuario, true, false);
            if (usuario == null)
            {
                Console.WriteLine("No se encontró un usuario con ese nombre");
                return;
            }
            Console.WriteLine("Id de la subasta:");
            int idPublicacion;
            if (!int.TryParse(Console.ReadLine(), out idPublicacion) || idPublicacion < 0)
            {
                Console.WriteLine("El ID de la subasta debe ser un número entero positivo");
                return;
            }
            Publicacion? publicacion = miSistema.ObtenerPublicacionPorId(idPublicacion, false, true);
            if (publicacion == null)
            {
                Console.WriteLine("No se encontró una subasta con ese ID");
                return;
            }
            Console.WriteLine("Monto a ofertar:");
            decimal monto;
            if (!decimal.TryParse(Console.ReadLine(), out monto) || monto <= 0)
            {
                Console.WriteLine("El monto debe ser un número decimal positivo");
                return;
            }

            miSistema.AltaOferta(usuario, publicacion, monto, fecha);
            Console.WriteLine("La oferta se ha realizado con éxito");
        }
        static void AltaOfertaPorNombre()
        {
            // Valores por defecto
            DateTime fecha = DateTime.Now;

            Console.Clear();
            // Solicitud datos
            Console.WriteLine("Ingrese los datos que desea asociar a la oferta");
            Console.WriteLine("Nombre del comprador:");
            string nombre = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombre)) 
            {
                Console.WriteLine("El nombre no puede ser vacío");
                return;
            }
            Usuario? usuario = miSistema.ObtenerUsuarioPorNombre(nombre, true, false);
            if (usuario == null)
            {
                Console.WriteLine("No se encontró un usuario con ese nombre");
                return;
            }
            Console.WriteLine("Id de la subasta:");
            int idPublicacion;
            if (!int.TryParse(Console.ReadLine(), out idPublicacion) || idPublicacion < 0) 
            {
                Console.WriteLine("El ID de la subasta debe ser un número entero positivo");
                return;
            }
            Publicacion? publicacion = miSistema.ObtenerPublicacionPorId(idPublicacion, false, true);
            if (publicacion == null)
            {
                Console.WriteLine("No se encontró una subasta con ese ID");
                return;
            }
            Console.WriteLine("Monto a ofertar:");
            decimal monto;
            if (!decimal.TryParse(Console.ReadLine(), out monto) || monto <= 0)
            {
                Console.WriteLine("El monto debe ser un número decimal positivo");
                return;
            }

            miSistema.AltaOferta(usuario, publicacion, monto, fecha);
            Console.WriteLine("La oferta se ha realizado con éxito");
        }
        #endregion
        #endregion
    }
}
