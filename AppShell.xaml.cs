using SmartMenu.Views;

namespace SmartMenu
{

    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrar la ruta para navegación
           // Routing.RegisterRoute(nameof(Views.Comida), typeof(Views.Comida));
        }
    }
}

