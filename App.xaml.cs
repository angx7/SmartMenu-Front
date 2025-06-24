using DotNetEnv;
using SmartMenu.Views;
using SmartMenu.Services;
using System.Diagnostics;

namespace SmartMenu;

public partial class App : Application
{
    private readonly AuthService _authService;

    public App()
    {
        Env.Load(".env");
        InitializeComponent();
        _authService = new AuthService();
        MainPage = new NavigationPage(new LoginPage()); // Página temporal mientras se inicializa
        InicializarAsync();
    }

    private async void InicializarAsync()
    {
        System.Diagnostics.Debug.WriteLine("Inicializando app...");
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null);
            System.Diagnostics.Debug.WriteLine("Token encontrado: " + token);
            if (token != null)
            {
                var rol = await _authService.ObtenerRolAsync(token);
                System.Diagnostics.Debug.WriteLine("Respuesta de ObtenerRolAsync: " + (rol ?? "null"));
                if (!string.IsNullOrEmpty(rol))
                {
                    System.Diagnostics.Debug.WriteLine("Tu rol es " + rol);
                    if (rol.Equals("administrador"))
                    {
                        /*pplication.Current.MainPage = new NavigationPage(new AdminView());*/
                    } else if (rol.Equals("cocinero"))
                    {
                        Application.Current.MainPage = new NavigationPage(new MeseroPage());
                    }
                    else
                    {
                        Application.Current.MainPage = new NavigationPage(new AppShell());
                    }
                    return;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No se pudo obtener el rol.");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Token es null.");
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("No hay token en Preferences.");
        }
        // Si no hay token o no se pudo obtener el rol, muestra el login
        MainPage = new NavigationPage(new LoginPage());
    }
}