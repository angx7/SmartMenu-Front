using DotNetEnv;
using SmartMenu.Views;

namespace SmartMenu;

public partial class App : Application
{
    public App()
    {
       
        InitializeComponent();
        
            // Si no hay token, muestra el login
            MainPage = new NavigationPage(new Views.CocinaPage());
        }
    }
