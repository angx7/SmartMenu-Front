using SmartMenu.Models;
using SmartMenu.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SmartMenu.ViewModels
{
    public class AddUserViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private readonly Page _page;

        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }

        public ICommand RegistrarCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AddUserViewModel(AuthService authService, Page page)
        {
            _authService = authService;
            _page = page;
            RegistrarCommand = new Command(async () => await Registrar());
        }

        private async Task Registrar()
        {
            var nuevo = new UserRequest
            {
                Nombre = Nombre,
                Usuario = Usuario,
                Contraseña = Contrasena,
                Rol_Id = Rol
            };

            var success = await _authService.RegistrarUsuario(nuevo);
            if (success)
            {
                await _page.DisplayAlert("Éxito", "Usuario registrado", "OK");
                await _page.Navigation.PopAsync();
            }
            else
            {
                await _page.DisplayAlert("Error", "No se pudo registrar", "OK");
            }
        }

        public class RolItem
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public ObservableCollection<RolItem> Roles { get; } = new()
        {
            new RolItem { Id = 1, Nombre = "Administrador" },
            new RolItem { Id = 2, Nombre = "Mesero" },
            new RolItem { Id = 3, Nombre = "Cocinero" }
        };

        private RolItem _rolSeleccionado;
        public RolItem RolSeleccionado
        {
            get => _rolSeleccionado;
            set
            {
                if (_rolSeleccionado != value)
                {
                    _rolSeleccionado = value;
                    Rol = value?.Id ?? 0; // Guarda el int en la propiedad Rol
                    OnPropertyChanged(nameof(RolSeleccionado));
                }
            }
        }

        private int _rol;
        public int Rol
        {
            get => _rol;
            set
            {
                if (_rol != value)
                {
                    _rol = value;
                    OnPropertyChanged(nameof(Rol));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}