using Microsoft.Maui.Controls;
using SmartMenu.Models;

namespace SmartMenu.Views;

public partial class PedidosPage : ContentPage
{
    public PedidosPage()
    {
        InitializeComponent();
    }

    private async void Pedido_Tapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is ComandaPedido pedido)
        {
            await Navigation.PushAsync(new DetallePedidoPage(pedido));
        }
    }
}