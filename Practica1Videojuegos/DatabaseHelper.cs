using MySql.Data.MySqlClient;

namespace Practica1Videojuegos
{
    /// <summary>
    /// Clase responsable de la cadena de conexión y de abrir/cerrar la conexión con MySQL.
    /// Toda la aplicación pide la conexión a esta clase, así está centralizada en un solo sitio.
    /// </summary>
    public static class DatabaseHelper
    {
        // IMPORTANTE: escribe en "Pwd" la contraseña de root de tu MySQL antes de ejecutar.
        // (No se sube la contraseña real al repositorio por seguridad)
        private const string CadenaConexion =
            "Server=localhost;Port=3306;Database=catalogo_videojuegos;Uid=root;Pwd=TU_CONTRASEÑA;";

        /// <summary>
        /// Crea y abre una conexión. Usar siempre con "using" para que se cierre sola.
        /// Si MySQL no está disponible lanza MySqlException (se captura en la ventana).
        /// </summary>
        public static MySqlConnection AbrirConexion()
        {
            var conexion = new MySqlConnection(CadenaConexion);
            conexion.Open();
            return conexion;
        }

        /// <summary>
        /// Cierra la conexión si está abierta.
        /// </summary>
        public static void CerrarConexion(MySqlConnection? conexion)
        {
            if (conexion != null && conexion.State != System.Data.ConnectionState.Closed)
            {
                conexion.Close();
            }
        }
    }
}
