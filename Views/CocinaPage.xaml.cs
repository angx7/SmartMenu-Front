using Microsoft.Maui.Storage;

namespace SmartMenu.Views;

public partial class CocinaPage : ContentPage
{
    public CocinaPage()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventarioPage());
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PedidosPage());
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Clear();
        await Task.Delay(100);
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }

    private async void OnAgregarInsumoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AgregarInsumoPage());
    }
}