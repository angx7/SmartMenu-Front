using SmartMenu.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;

namespace SmartMenu.Views;
public partial class meserosPage : ContentPage
{
    private const string apiUrl = "https://da63-2a09-bac1-50c0-60-00-9f-37.ngrok-free.app/api/usuarios/rol/2";

    // Token fijo (solo para pruebas)
    private const string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MSwidXN1YXJpbyI6ImFkbWluIiwicm9sX2lkIjoxLCJpYXQiOjE3NTA3MzEzNTIsImV4cCI6MTc1MDc2MDE1Mn0.-7jtzX-iCQeXbmjayXmYl1ZSaKMpk57ir39rGgO3Smo";

    public meserosPage()
    {
        InitializeComponent();
        CargarMeseros();
    }

    private async void CargarMeseros()
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
            var meseros = JsonConvert.DeserializeObject<List<Mesero>>(json);

            if (meseros is null || meseros.Count == 0)
            {
                await DisplayAlert("Atención", "No se encontraron meseros.", "OK");
                return;
            }

            foreach (var mesero in meseros)
            {
                var boton = new Button
                {
                    Text = $"{mesero.Nombre} - {mesero.Telefono}",
                    BackgroundColor = Colors.Red,
                    TextColor = Colors.White,
                    CornerRadius = 20
                };

                boton.Clicked += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(mesero.Telefono))
                        PhoneDialer.Open(mesero.Telefono);
                    else
                        DisplayAlert("Sin teléfono", "Este mesero no tiene número registrado.", "OK");
                };

                meserosLayout.Children.Add(boton);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }
}
