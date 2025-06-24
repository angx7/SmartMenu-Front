using Microsoft.Maui.Controls;
using SmartMenu.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartMenu.Views
{
    public partial class TicketPage : ContentPage
    {
        public TicketPage(int mesaId, int pedidoId, List<DetallePedido> detalles)
        {
            InitializeComponent();

            PedidoInfo.Text = $"Pedido #{pedidoId} - Mesa {mesaId}";

            var detallesConSubtotal = detalles.Select(d => new DetalleTicket
            {
                nombre = d.nombre,
                precio = d.precio,
                cantidad = d.cantidad,
                subtotal = d.precio * d.cantidad
            }).ToList();

            DetallesList.ItemsSource = new ObservableCollection<DetalleTicket>(detallesConSubtotal);

            var total = detallesConSubtotal.Sum(d => d.subtotal);
            TotalLabel.Text = $"Total: ${total:F2}";
        }

        private async void VolverInicio_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        public class DetalleTicket
        {
            public string nombre { get; set; }
            public double precio { get; set; }
            public int cantidad { get; set; }
            public double subtotal { get; set; }
        }
    }
}
