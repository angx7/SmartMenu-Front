
using Microsoft.Maui.Controls;

namespace SmartMenu.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
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

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Clear();
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
}