
namespace SmartMenu.Views;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;



public partial class CarritoPage : ContentPage { 
    
        private List<Comida.Platillo> carrito;
        private int mesaId;

        public CarritoPage(List<Comida.Platillo> carrito, int mesaId)
        {
            InitializeComponent();
            this.carrito = carrito;
            this.mesaId = mesaId;

            Title = $"Mesa {mesaId}";
            ListaCarrito.ItemsSource = carrito;

            decimal total = carrito.Sum(p => p.Precio);
            TotalLabel.Text = $"Total: ${total:F2}";
        }

        private async void ConfirmarPedido_Clicked(object sender, EventArgs e)
        {
            // Aquí puedes guardar en BD o imprimir, por ahora solo simula
            await DisplayAlert("Confirmado", "El pedido fue confirmado con éxito.", "OK");
            await Navigation.PopToRootAsync(); // Regresa a la pantalla principal
        }
    }
