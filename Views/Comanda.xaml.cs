using Newtonsoft.Json;
using SmartMenu.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace SmartMenu.Views
{
    public partial class ComandaPage : ContentPage
    {
        private readonly int _mesaId;
        private readonly int _pedidoId;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private ObservableCollection<DetallePedido> _platillos;
        private double _total;

        public ComandaPage(int mesaId, int pedidoId, bool nuevo, List<DetallePedido> detalles, double total)
        {
            InitializeComponent();
            _mesaId = mesaId;
            _pedidoId = pedidoId;
            _httpClient = new HttpClient();
            _baseUrl = Environment.GetEnvironmentVariable("NGROK_URL") ?? "https://da63-2a09-bac1-50c0-60-00-9f-37.ngrok-free.app";
            _platillos = new ObservableCollection<DetallePedido>(detalles);
            PlatillosList.ItemsSource = _platillos;
            _total = total;
            TotalLabel.Text = $"Total: ${_total:F2}";
        }

        private async void OnAgregarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Comida(_mesaId, _pedidoId, false, _platillos.ToList(), _total));
        }

        private async void OnPagarClicked(object sender, EventArgs e)
        {
            try
            {
                var response = await _httpClient.PutAsync($"{_baseUrl}/api/pedidos/{_pedidoId}/finalizar", null);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Cuenta pagada correctamente.", "OK");
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"No se pudo pagar la cuenta.\n\n{content}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }

    public class PedidoMesaResponse
    {
        public int pedido_id { get; set; }
        public bool nuevo { get; set; }
        public List<DetallePedido> detalles { get; set; }
        public double total { get; set; }
    }
}
