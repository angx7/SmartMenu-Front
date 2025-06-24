using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SmartMenu.Models;
using Microsoft.Maui.Storage;

namespace SmartMenu.Views;

public partial class HomePage : ContentPage
{
    private readonly HttpClient _httpClient;
    private readonly string BaseUrl;

    public HomePage()
    {
        InitializeComponent();
        _httpClient = new HttpClient();

        // Igual que AuthService
        BaseUrl = Environment.GetEnvironmentVariable("NGROK_URL")
                   ?? "https://da63-2a09-bac1-50c0-60-00-9f-37.ngrok-free.app";
    }

    private async Task IrAMesa(int mesaId)
    {
        var usuarioId = Preferences.Get("usuario_id", 0);
        if (usuarioId == 0)
        {
            usuarioId = 2; // Usuario por defecto
        }

        var body = new
        {
            usuario_id = usuarioId,
            cliente_id = (int?)null
        };

        var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        try
        {
            var url = $"{BaseUrl}/api/pedidos/mesa/{mesaId}";
            var response = await _httpClient.PostAsync(url, content);
            var responseText = await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine($"🔴 Status: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"🔴 Body: {responseText}");

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", $"No se pudo obtener o crear el pedido\n\n{responseText}", "OK");
                return;
            }

            var data = JsonConvert.DeserializeObject<PedidoMesaResponse>(responseText);

            await Navigation.PushAsync(new ComandaPage(mesaId, data.pedido_id, data.nuevo, data.detalles, data.total));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void Mesa1_Clicked(object sender, EventArgs e) => await IrAMesa(1);
    private async void Mesa2_Clicked(object sender, EventArgs e) => await IrAMesa(2);
    private async void Mesa3_Clicked(object sender, EventArgs e) => await IrAMesa(3);

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Clear();
        await Task.Delay(100);
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
}
