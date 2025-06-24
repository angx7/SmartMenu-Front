
using Microsoft.Maui.Controls;

namespace SmartMenu.Views;

public partial class MeseroPage : ContentPage
{
    public MeseroPage()
    {
        InitializeComponent();
    }

    private async void Mesa1_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Comida(1));
    }

    private async void Mesa2_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Comida(2));
    }

    private async void Mesa3_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Comida(3));
    }

   private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Clear();
        await Task.Delay(100); // Da tiempo a que se cierren tareas pendientes
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }

}