using Microsoft.Maui.Controls;
using SmartMenu.Models;

namespace SmartMenu.Views
{
    public partial class DetallePedidoPage : ContentPage
    {
        public DetallePedidoPage(ComandaPedido pedido)
        {
            InitializeComponent();
            BindingContext = pedido;
        }
    }
}