using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartMenu.Models;
using Microsoft.Maui.Storage;

namespace SmartMenu.ViewModels
{
    public class PedidosViewModel
    {
        public ObservableCollection<ComandaPedido> Pedidos { get; set; } = new();
        public bool IsBusy { get; set; }

        public PedidosViewModel()
        {
            _ = CargarPedidos();
        }

        public async Task CargarPedidos()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrWhiteSpace(token)) return;

                var baseUrl = Environment.GetEnvironmentVariable("NGROK_URL");
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"{baseUrl}/api/pedidos/comandas");
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) return;

                var pedidos = JsonConvert.DeserializeObject<List<ComandaPedido>>(json);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Pedidos.Clear();
                    foreach (var p in pedidos)
                        Pedidos.Add(p);
                });
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}