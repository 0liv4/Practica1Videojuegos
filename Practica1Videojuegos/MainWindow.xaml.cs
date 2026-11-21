using System.Windows;
    
namespace Practica1Videojuegos
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Este es el método que echaba en falta el programa
        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            // Abre de nuevo la ventana de Login
            LoginWindow ventanaLogin = new LoginWindow();
            ventanaLogin.Show();

            // Cierra el catálogo principal
            this.Close();
        }
    }
}