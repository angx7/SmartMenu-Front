using Microsoft.Maui.Controls;
using SmartMenu.Models;

namespace SmartMenu.Views;

public partial class PedidosPage : ContentPage
{
    public PedidosPage()
    {
        InitializeComponent();
    }

    private async void PedidosCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ComandaPedido pedido)
        {
            await Navigation.PushAsync(new DetallePedidoPage(pedido));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}