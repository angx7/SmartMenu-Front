namespace SmartMenu.Views;

public partial class AdminView : ContentPage
{
	public AdminView()
	{
		InitializeComponent();
	}

    private async void OnAgregarUsuarioClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddUserPage());
    }

    private async void OnAgregarProveedorClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new registarProveedores());
    }

    private async void OnVerMeserosDisponibles(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new usuariosPage());
    }

    private async void OnVerProveedoresDisponibles(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new proveedoresPage());
    }
    private void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Clear();
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
}