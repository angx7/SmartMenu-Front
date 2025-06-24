using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using SmartMenu.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Maui.Storage;

namespace SmartMenu.Views
{
    public partial class ClientesPage : ContentPage
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ClientesPage()
        {
            InitializeComponent();

            _httpClient = new HttpClient();
            _baseUrl = Environment.GetEnvironmentVariable("NGROK_URL") ?? "https://tu-ngrok.ngrok-free.app";

            var token = Preferences.Get("token", null);
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            CargarClientes();
        }

        private async void CargarClientes()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/clientes");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"No se pudieron obtener los clientes.\n{error}", "OK");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<Cliente>>(json);

                ClientesCollection.ItemsSource = clientes;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnAgregarClienteClicked(object sender, EventArgs e)
        {
            var nombre = NombreEntry.Text?.Trim();
            var correo = CorreoEntry.Text?.Trim();
            var telefono = TelefonoEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(telefono))
            {
                await DisplayAlert("Campos requeridos", "Completa todos los campos.", "OK");
                return;
            }

            // Validación de correo
            if (!Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                await DisplayAlert("Correo inválido", "Introduce un correo electrónico válido.", "OK");
                return;
            }

            // Validación de teléfono: solo números y 10 dígitos
            if (!Regex.IsMatch(telefono, @"^\d{10}$"))
            {
                await DisplayAlert("Teléfono inválido", "El teléfono debe tener exactamente 10 dígitos.", "OK");
                return;
            }

            var nuevoCliente = new
            {
                nombre,
                email = correo,
                telefono
            };

            var json = JsonConvert.SerializeObject(nuevoCliente);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}/api/clientes", content);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Cliente agregado correctamente", "OK");

                    NombreEntry.Text = "";
                    CorreoEntry.Text = "";
                    TelefonoEntry.Text = "";

                    CargarClientes();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"No se pudo agregar el cliente.\n{error}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }

    public class Cliente
    {
        public string nombre { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
    }
}
