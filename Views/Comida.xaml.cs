using Microsoft.Maui.Controls;
using SmartMenu.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmartMenu.Views
{
    public partial class Comida : ContentPage
    {
        private readonly int _mesaId;
        private readonly int _pedidoId;
        private readonly ObservableCollection<DetallePedido> _platillosActuales;
        private readonly double _total;
        private readonly List<Platillo> _platillosDisponibles;

        private readonly List<Platillo> carrito = new();

        public Comida(int mesaId, int pedidoId, bool nuevo, List<DetallePedido> detalles, double total, List<Platillo> platillos)
        {
            InitializeComponent();
            _mesaId = mesaId;
            _pedidoId = pedidoId;
            _platillosActuales = new ObservableCollection<DetallePedido>(detalles);
            _total = total;
            _platillosDisponibles = platillos;

            PlatillosCollection.ItemsSource = _platillosDisponibles;
        }

        private void AgregarAlCarrito_Clicked(object sender, EventArgs e)
        {
            var boton = sender as Button;
            var platillo = boton?.CommandParameter as Platillo;

            if (platillo != null)
            {
                carrito.Add(platillo);
                DisplayAlert("Agregado", $"{platillo.nombre} añadido al carrito", "OK");
            }
        }

        private async void AbrirCarrito_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CarritoPage(carrito, _pedidoId));
        }

    }
}
