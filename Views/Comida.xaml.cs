using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartMenu.Views
{
    public partial class Comida : ContentPage
    {
        // Modelo del platillo actualizado
        public class Platillo
        {
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
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

            // Lista de platillos con descripciones
            platillos.Add(new Platillo
            {
                Nombre = "Hamburguesa",
                Descripcion = "Clásica hamburguesa con carne de res, queso y vegetales.",
                Precio = 120
            });

            platillos.Add(new Platillo
            {
                Nombre = "Ensalada",
                Descripcion = "Ensalada fresca con lechuga, jitomate, pepino y aderezo.",
                Precio = 90
            });

            platillos.Add(new Platillo
            {
                Nombre = "Pizza Pepperoni",
                Descripcion = "Pizza con salsa de tomate, queso mozzarella y pepperoni.",
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
