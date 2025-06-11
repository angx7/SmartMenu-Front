using SmartMenu.Models;
using SmartMenu.Services;
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
        public string Rol { get; set; }

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
            int rolId = 0;
            int.TryParse(Rol, out rolId); // evita que truene si escriben letras

            var nuevo = new UserRequest
            {
                Nombre = Nombre,
                Usuario = Usuario,
                Contraseña = Contrasena,
                Rol_Id = rolId
            };

            var success = await _authService.RegistrarUsuario(nuevo);
            if (success)
            {
                await _page.DisplayAlert("Éxito", "Usuario registrado", "OK");
                await _page.Navigation.PopAsync();
            } else
                await _page.DisplayAlert("Error", "No se pudo registrar", "OK");
        }
    }
}
