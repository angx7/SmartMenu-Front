using DotNetEnv;
using SmartMenu.Views;

namespace SmartMenu;

public partial class App : Application
{
    public App()
    {
        Env.Load(".env");
        InitializeComponent();
        // Si hay token guardado, va directo al Shell (donde está HomePage)
        if (Preferences.ContainsKey("token"))
        {
            MainPage = new AppShell();
        }
        else
        {
            // Si no hay token, muestra el login
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}