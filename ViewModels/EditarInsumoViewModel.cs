using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SmartMenu.Models;
using SmartMenu.Services;

namespace SmartMenu.ViewModels
{
    public class EditarInsumoViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private readonly INavigation _navigation;
        private readonly InventarioViewModel _inventarioViewModel;

        public int Id { get; }
        public string NombreOriginal { get; }
        public string UnidadOriginal { get; }
        public string StockMinimoOriginal { get; }

        private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); }
        }

        private string _stock;
        public string Stock
        {
            get => _stock;
            set { _stock = value; OnPropertyChanged(); }
        }

        private string _unidad;
        public string Unidad
        {
            get => _unidad;
            set { _unidad = value; OnPropertyChanged(); }
        }

        private string _stockMinimo;
        public string StockMinimo
        {
            get => _stockMinimo;
            set { _stockMinimo = value; OnPropertyChanged(); }
        }

        public ICommand ConfirmarCommand { get; }
        public ICommand EliminarCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public EditarInsumoViewModel(Insumo insumo, INavigation navigation, InventarioViewModel inventarioViewModel)
        {
            _authService = new AuthService();
            _navigation = navigation;
            _inventarioViewModel = inventarioViewModel;

            Id = insumo.Id;
            Nombre = insumo.Nombre;
            Stock = insumo.Stock;
            Unidad = insumo.Unidad;
            StockMinimo = insumo.StockMinimo;

            ConfirmarCommand = new Command(async () => await Confirmar());
            EliminarCommand = new Command(async () => await Eliminar());
        }

        private async Task Confirmar()
        {
            if (string.IsNullOrWhiteSpace(Nombre) ||
                string.IsNullOrWhiteSpace(Stock) ||
                string.IsNullOrWhiteSpace(Unidad) ||
                string.IsNullOrWhiteSpace(StockMinimo))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
                return;
            }

            // Validar que Stock y StockMinimo sean números decimales válidos
            if (!decimal.TryParse(Stock, out var stockDecimal) ||
                !decimal.TryParse(StockMinimo, out var stockMinDecimal))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Stock y Stock Mínimo deben ser números válidos.", "OK");
                return;
            }

            var insumoActualizado = new Insumo
            {
                Id = Id,
                Nombre = Nombre,
                Stock = stockDecimal.ToString(), // Si tu backend espera string, envía como string
                Unidad = Unidad,
                StockMinimo = stockMinDecimal.ToString()
            };

            var exito = await _authService.ActualizarInsumoAsync(insumoActualizado);
            if (exito)
            {
                await App.Current.MainPage.DisplayAlert("Éxito", "Insumo actualizado", "OK");
                await _inventarioViewModel.Refrescar();
                await _navigation.PopAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar el insumo", "OK");
            }
        }

        private async Task Eliminar()
        {
            var confirmar = await App.Current.MainPage.DisplayAlert("Eliminar", "¿Seguro que deseas eliminar este insumo?", "Sí", "No");
            if (!confirmar) return;

            var exito = await _authService.EliminarInsumoAsync(Id);
            if (exito)
            {
                await App.Current.MainPage.DisplayAlert("Eliminado", "Insumo eliminado", "OK");
                await _inventarioViewModel.Refrescar();
                await _navigation.PopAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar el insumo", "OK");
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}