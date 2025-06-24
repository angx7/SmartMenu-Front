using SmartMenu.Models;
using SmartMenu.Services;

namespace SmartMenu.Views;

public partial class proveedoresPage : ContentPage
{
    private readonly AuthService _authService = new();

    public proveedoresPage()
    {
        InitializeComponent();
        CargarProveedores();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarProveedores();
    }

    private async void CargarProveedores()
    {
        try
        {
            var proveedores = await _authService.ObtenerProveedoresAsync();

            if (proveedores is null || proveedores.Count == 0)
            {
                await DisplayAlert("Atención", "No se encontraron proveedores.", "OK");
                return;
            }

            ProveedoresLayout.Children.Clear();

            foreach (var proveedor in proveedores)
            {
                var info = $"{proveedor.Nombre}\nContacto: {proveedor.Contacto}\nTel: {proveedor.Telefono}\nCorreo: {proveedor.Correo}";

                var boton = new Button
                {
                    Text = info,
                    BackgroundColor = Colors.Red,
                    TextColor = Colors.White,
                    CornerRadius = 20,
                    Padding = new Thickness(10),
                    FontSize = 14
                };

                // Navegación a edición
                boton.Clicked += async (s, e) =>
                {
                    await Navigation.PushAsync(new EditarProveedorPage(proveedor));
                };

                ProveedoresLayout.Children.Add(boton);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }
}