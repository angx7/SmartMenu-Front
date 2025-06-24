using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartMenu.Views
{
    public partial class Comida : ContentPage
    {
        // Modelo del platillo
        public class Platillo
        {
            public string Nombre { get; set; }
            public string Imagen { get; set; }
            public decimal Precio { get; set; }
        }

        private List<Platillo> carrito = new();
        private List<Platillo> platillos = new();
        private int mesaId;

        public Comida(int mesaId)
        {
            InitializeComponent();
            this.mesaId = mesaId;
            Title = $"Pedido - Mesa {mesaId}";

            // Inicializa la lista de platillos (simulados)
            platillos.Add(new Platillo
            {
                Nombre = "Hamburguesa",
                Imagen = "hamburguesa.png",
                Precio = 120
            });

            platillos.Add(new Platillo
            {
                Nombre = "Ensalada",
                Imagen = "ensalada.png",
                Precio = 90
            });

            platillos.Add(new Platillo
            {
                Nombre = "Pizza Pepperoni",
                Imagen = "pizza.png",
                Precio = 150
            });

            // Enlaza al CollectionView
            PlatillosCollection.ItemsSource = platillos;
        }

        private void AgregarAlCarrito_Clicked(object sender, EventArgs e)
        {
            var boton = sender as Button;
            var platillo = boton?.CommandParameter as Platillo;

            if (platillo != null)
            {
                carrito.Add(platillo);
                DisplayAlert("Agregado", $"{platillo.Nombre} añadido al carrito", "OK");
            }
        }

        private async void AbrirCarrito_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CarritoPage(carrito, mesaId));
        }
    }
}