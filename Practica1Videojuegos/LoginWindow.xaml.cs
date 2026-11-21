using System.Windows;
using MySql.Data.MySqlClient;

namespace Practica1Videojuegos
{
    public partial class LoginWindow : Window
    {
        // Práctica 3: se elimina la lista de usuarios en memoria.
        // Las credenciales se validan contra la tabla Usuarios de MySQL.

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string user = txtUsuario.Text.Trim();
            string pass = txtPassword.Password;

            // TC 01: Campos vacíos. Se muestra el mensaje y NO se consulta la BD
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                lblMensaje.Foreground = System.Windows.Media.Brushes.Orange;
                lblMensaje.Content = "Introduzca datos";
                MessageBox.Show("Introduzca datos", "Inicio de sesión",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using MySqlConnection conexion = DatabaseHelper.AbrirConexion();

                // Consulta parametrizada: comprueba si el usuario y la contraseña existen en la tabla.
                // Los valores van como parámetros, nunca concatenados (TC 09: evita la inyección SQL)
                string sql = "SELECT COUNT(*) FROM Usuarios WHERE nombre_usuario = @user AND password = @pass";
                using MySqlCommand comando = new MySqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@user", user);
                comando.Parameters.AddWithValue("@pass", pass);

                long coincidencias = Convert.ToInt64(comando.ExecuteScalar());

                if (coincidencias > 0)
                {
                    // TC 03 / TC 07: Inicio de sesión exitoso -> se abre la Home del catálogo
                    lblMensaje.Foreground = System.Windows.Media.Brushes.LightGreen;
                    lblMensaje.Content = "¡Login correcto!";

                    MainWindow ventanaPrincipal = new MainWindow();
                    ventanaPrincipal.Show();
                    this.Close();
                }
                else
                {
                    // TC 04 / TC 09: Credenciales incorrectas
                    lblMensaje.Foreground = System.Windows.Media.Brushes.Red;
                    lblMensaje.Content = "Usuario o contraseña incorrectos";
                }
            }
            catch (MySqlException)
            {
                // TC 08: MySQL no disponible. Se avisa y la aplicación sigue abierta
                lblMensaje.Foreground = System.Windows.Media.Brushes.Red;
                lblMensaje.Content = "Error de conexión con la base de datos";
                MessageBox.Show("No se ha podido conectar con la base de datos.\nCompruebe que el servicio de MySQL está iniciado.",
                                "Error de conexión", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Práctica 3 - TC 05 / TC 06: Alta de nuevo usuario en la tabla Usuarios
        private void BtnNuevoUsuario_Click(object sender, RoutedEventArgs e)
        {
            string user = txtUsuario.Text.Trim();
            string pass = txtPassword.Password;

            // Validación de campos vacíos: no se consulta la BD
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                lblMensaje.Foreground = System.Windows.Media.Brushes.Orange;
                lblMensaje.Content = "Introduzca datos";
                MessageBox.Show("Introduzca datos", "Nuevo usuario",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using MySqlConnection conexion = DatabaseHelper.AbrirConexion();

                // Consulta parametrizada: los valores nunca se concatenan al SQL (evita inyección SQL)
                string sql = "INSERT INTO Usuarios (nombre_usuario, password) VALUES (@user, @pass)";
                using MySqlCommand comando = new MySqlCommand(sql, conexion);
                comando.Parameters.AddWithValue("@user", user);
                comando.Parameters.AddWithValue("@pass", pass);
                comando.ExecuteNonQuery();

                // TC 05: alta correcta
                lblMensaje.Foreground = System.Windows.Media.Brushes.LightGreen;
                lblMensaje.Content = "Usuario dado de alta correctamente";
                MessageBox.Show($"El usuario \"{user}\" se ha dado de alta correctamente.",
                                "Nuevo usuario", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                // TC 06: 1062 = entrada duplicada (nombre_usuario es UNIQUE)
                lblMensaje.Foreground = System.Windows.Media.Brushes.Red;
                lblMensaje.Content = "El usuario ya existe";
            }
            catch (MySqlException)
            {
                // TC 08: MySQL no disponible. Se avisa y la aplicación sigue abierta
                lblMensaje.Foreground = System.Windows.Media.Brushes.Red;
                lblMensaje.Content = "Error de conexión con la base de datos";
                MessageBox.Show("No se ha podido conectar con la base de datos.\nCompruebe que el servicio de MySQL está iniciado.",
                                "Error de conexión", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // TC 06 / TC 07: Salir de la aplicación
        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            // Cierra toda la aplicación
            Application.Current.Shutdown();
        }
    }
}