using DotNetEnv;
using SmartMenu.Views;

namespace SmartMenu;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Esto elimina el uso de AppShell y usa NavigationPage con HomePage como raíz
        MainPage = new NavigationPage(new Views.HomePage());
    }
}

