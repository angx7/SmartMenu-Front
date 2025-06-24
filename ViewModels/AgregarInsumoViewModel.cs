using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SmartMenu.Models;
using SmartMenu.Services;

namespace SmartMenu.ViewModels
{
    public class AgregarInsumoViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private readonly INavigation _navigation;

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

        public ICommand GuardarCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AgregarInsumoViewModel(INavigation navigation)
        {
            _authService = new AuthService();
            _navigation = navigation;
            GuardarCommand = new Command(async () => await Guardar());
        }

        private async Task Guardar()
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

            var nuevo = new Insumo
            {
                Nombre = Nombre,
                Stock = stockDecimal.ToString(),
                Unidad = Unidad,
                StockMinimo = stockMinDecimal.ToString()
            };

            var exito = await _authService.CrearInsumoAsync(nuevo);
            if (exito)
            {
                await App.Current.MainPage.DisplayAlert("Éxito", "Insumo creado", "OK");
                await _navigation.PopAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "No se pudo crear el insumo", "OK");
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}