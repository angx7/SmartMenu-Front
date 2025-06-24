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
            var datos = JsonConvert.DeserializeObject<List<Mesero>>(response);

            var proveedores = datos.Where(p => p.Rol?.ToLower() == "proveedor").ToList();

            foreach (var proveedor in proveedores)
            {
                var boton = new Button
                {
                    Text = $"{proveedor.Nombre} - {proveedor.Telefono}",
                    BackgroundColor = Colors.Red,
                    TextColor = Colors.White,
                    CornerRadius = 20
                };

                boton.Clicked += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(proveedor.Telefono))
                        PhoneDialer.Open(proveedor.Telefono);
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