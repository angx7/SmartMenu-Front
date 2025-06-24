using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using SmartMenu.Services;
using SmartMenu.Models;

namespace SmartMenu.ViewModels
{
    public class RegistrarProveedorViewModel : INotifyPropertyChanged
    {
        private readonly Page _page;
        private readonly AuthService _authService;

        public string Nombre { get; set; }
        public string Contacto { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }

        public ICommand RegistrarCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RegistrarProveedorViewModel(AuthService authService, Page page)
        {
            _authService = authService;
            _page = page;
            RegistrarCommand = new Command(async () => await Registrar());
        }

        private async Task Registrar()
        {
            // Aquí deberías crear tu modelo de proveedor y llamar a tu servicio para registrar
            // Por ejemplo, si tienes un modelo ProveedorRequest y un método RegistrarProveedor en tu servicio:
            var nuevoProveedor = new ProveedorRequest
            {
                Nombre = Nombre,
                Contacto = Contacto,
                Telefono = Telefono,
                Correo = Correo
            };

            // Suponiendo que tienes un método similar a RegistrarUsuario:
            var success = await _authService.RegistrarProveedor(nuevoProveedor);
            if (success)
            {
                await _page.DisplayAlert("Éxito", "Proveedor registrado", "OK");
                await _page.Navigation.PopAsync();
            }
            else
            {
                await _page.DisplayAlert("Error", "No se pudo registrar el proveedor", "OK");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}