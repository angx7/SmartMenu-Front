using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace SmartMenu.Views
{
    public partial class Comida : ContentPage
    {
        int mesaId;

        public Comida(int mesaId)
        {
            InitializeComponent();
            this.mesaId = mesaId;
            Title = $"Pedido - Mesa {mesaId}";
        }
    }

}
