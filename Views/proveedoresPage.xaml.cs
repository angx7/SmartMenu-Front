namespace SmartMenu.Views;
using SmartMenu.Models;
using Newtonsoft.Json;

public partial class proveedoresPage : ContentPage
{
    private const string apiUrl = "https://da63-2a09-bac1-50c0-60-00-9f-37.ngrok-free.app/api/clientes";

    public proveedoresPage()
    {
        InitializeComponent();
        CargarProveedores();
    }

    private async void CargarProveedores()
    {
        try
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync(apiUrl);
            var datos = JsonConvert.DeserializeObject<List<Usuario>>(response);

            var proveedores = datos.Where(p => p.Rol_Id == 2).ToList();

            foreach (var proveedor in proveedores)
            {
                var boton = new Button
                {
                    Text = $"{proveedor.Nombre} - {proveedor.Rol_Id}",
                    BackgroundColor = Colors.Red,
                    TextColor = Colors.White,
                    CornerRadius = 20
                };

                boton.Clicked += (s, e) =>
                {
                    if (proveedor.Rol_Id == null)
                        PhoneDialer.Open(proveedor.Contraseña);
                };

                ProveedoresLayout.Children.Add(boton);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo cargar proveedores: {ex.Message}", "OK");
        }
    }
}