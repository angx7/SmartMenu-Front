using Microsoft.Maui.Controls;
using SmartMenu.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;
using System.Linq;
using System.ComponentModel;

namespace SmartMenu.Views
{
    public partial class CarritoPage : ContentPage
    {
        private readonly int _pedidoId;
        private readonly ObservableCollection<PlatilloCarrito> _carrito;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public CarritoPage(List<Platillo> productosSeleccionados, int pedidoId)
        {
            InitializeComponent();
            _pedidoId = pedidoId;

            _carrito = new ObservableCollection<PlatilloCarrito>();
            foreach (var platillo in productosSeleccionados)
            {
                var item = new PlatilloCarrito
                {
                    platillo_id = platillo.platillo_id,
                    nombre = platillo.nombre,
                    precio = platillo.precio,
                    cantidad = 1
                };

                item.PropertyChanged += PlatilloCarrito_PropertyChanged;
                _carrito.Add(item);
            }

            CarritoCollection.ItemsSource = _carrito;
            ActualizarTotal();

            _httpClient = new HttpClient();
            _baseUrl = Environment.GetEnvironmentVariable("NGROK_URL") ?? "https://tu-ngrok-url.ngrok-free.app";

            var token = Preferences.Get("token", null);
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        private void PlatilloCarrito_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlatilloCarrito.cantidad))
            {
                ActualizarTotal();
            }
        }

        private void ActualizarTotal()
        {
            double total = _carrito.Sum(p => p.Subtotal);
            TotalLabel.Text = $"Total: ${total:F2}";
        }

        private async void OnConfirmarPedidoClicked(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in _carrito)
                {
                    var payload = new
                    {
                        platillo_id = item.platillo_id,
                        cantidad = item.cantidad
                    };

                    var json = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    //var response = await _httpClient.PostAsync($"{_baseUrl}/api/pedidos/{_pedidoId}/platillos", content);
                    var url = $"{_baseUrl}/api/pedidos/{_pedidoId}/platillos";
                    System.Diagnostics.Debug.WriteLine($"URL: {url}");
                    System.Diagnostics.Debug.WriteLine($"Payload: {json}");

                    var response = await _httpClient.PostAsync(url, content);


                    if (!response.IsSuccessStatusCode)
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        await DisplayAlert("Error", $"Fallo al enviar {item.nombre}:\n{error}", "OK");
                        return;
                    }
                }

                await DisplayAlert("Éxito", "Pedido actualizado correctamente", "OK");
                await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }

    public class PlatilloCarrito : INotifyPropertyChanged
    {
        public int platillo_id { get; set; }
        public string nombre { get; set; }
        public double precio { get; set; }

        private int _cantidad = 1;
        public int cantidad
        {
            get => _cantidad;
            set
            {
                if (_cantidad != value)
                {
                    _cantidad = value;
                    OnPropertyChanged(nameof(cantidad));
                    OnPropertyChanged(nameof(Subtotal));
                }
            }
        }

        public double Subtotal => precio * cantidad;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
