using SmartMenu.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;

namespace SmartMenu.Views;

public partial class proveedoresPage : ContentPage
{
    private const string apiUrl = "https://da63-2a09-bac1-50c0-60-00-9f-37.ngrok-free.app/api/proveedores";

    private const string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MSwidXN1YXJpbyI6ImFkbWluIiwicm9sX2lkIjoxLCJpYXQiOjE3NTA3NDMwOTUsImV4cCI6MTc1MDc3MTg5NX0.9CUl5W6eHn7HY0a2enu_d9nP9GwElG-W1DBlOFL-0GQ";

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
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(apiUrl);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                await DisplayAlert("Error", "No tienes permiso para acceder a esta ruta (403).", "OK");
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", $"Error del servidor: {response.StatusCode}", "OK");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var proveedores = JsonConvert.DeserializeObject<List<Mesero>>(json); // usando el modelo base

            if (proveedores is null || proveedores.Count == 0)
            {
                await DisplayAlert("Atención", "No se encontraron proveedores.", "OK");
                return;
            }

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

                boton.Clicked += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(proveedor.Telefono))
                        PhoneDialer.Open(proveedor.Telefono);
                    else
                        DisplayAlert("Sin teléfono", "Este proveedor no tiene número registrado.", "OK");
                };

                ProveedoresLayout.Children.Add(boton);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }
}
