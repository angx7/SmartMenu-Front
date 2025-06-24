using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SmartMenu.Models;
using SmartMenu.Services;
using SmartMenu.Views;

namespace SmartMenu.ViewModels
{
    public class InventarioViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private bool _isBusy;

        public ObservableCollection<Insumo> Insumos { get; } = new();
        public ICommand CargarInsumosCommand { get; }
        public ICommand EditarInsumoCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public InventarioViewModel()
        {
            _authService = new AuthService();
            CargarInsumosCommand = new Command(async () => await CargarInsumos());
            EditarInsumoCommand = new Command<Insumo>(async (insumo) => await AbrirEditarInsumo(insumo));
        }

        public async Task CargarInsumos()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var insumos = await _authService.ObtenerInsumosAsync();
                Insumos.Clear();
                if (insumos != null)
                {
                    foreach (var insumo in insumos)
                        Insumos.Add(insumo);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudieron cargar los insumos.", "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AbrirEditarInsumo(Insumo insumo)
        {
            // Navega a la ventana de edición, pasando el insumo seleccionado
            await Application.Current.MainPage.Navigation.PushAsync(new EditarInsumoPage(insumo, this));
        }

        // Método para refrescar desde EditarInsumoPage
        public async Task Refrescar()
        {
            await CargarInsumos();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}