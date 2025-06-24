using System.ComponentModel;
using System.Windows.Input;
using SmartMenu.Models;
using SmartMenu.Services;

namespace SmartMenu.ViewModels;

public class EditarUsuarioViewModel : INotifyPropertyChanged
{
    private readonly AuthService _authService;
    private readonly Page _page;

    public event PropertyChangedEventHandler PropertyChanged;

    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Usuario { get; set; }
    public int Rol { get; set; }

    // Propiedad para el Picker (índice base 0)
    public int RolIndex
    {
        get => Rol - 1;
        set
        {
            if (Rol != value + 1)
            {
                Rol = value + 1;
                OnPropertyChanged(nameof(RolIndex));
                OnPropertyChanged(nameof(Rol));
            }
        }
    }

    public ICommand GuardarCommand { get; }
    public ICommand EliminarCommand { get; }

    private Usuario _usuarioOriginal;

    public EditarUsuarioViewModel(AuthService authService, Page page, Usuario usuario)
    {
        _authService = authService;
        _page = page;
        _usuarioOriginal = usuario;

        Id = usuario.Id;
        Nombre = usuario.Nombre;
        Usuario = usuario.NombreUsuario;
        Rol = usuario.Rol_Id;

        GuardarCommand = new Command(async () => await Guardar());
        EliminarCommand = new Command(async () => await Eliminar());
    }

    private async Task Guardar()
    {
        var actualizado = new Usuario
        {
            Id = Id,
            Nombre = Nombre,
            NombreUsuario = Usuario,
            Rol_Id = Rol
        };

        var result = await _authService.ActualizarUsuarioAsync(actualizado);
        if (result)
        {
            await _page.DisplayAlert("Éxito", "Usuario actualizado", "OK");
            await _page.Navigation.PopAsync();
        }
        else
        {
            await _page.DisplayAlert("Error", "No se pudo actualizar", "OK");
        }
    }

    private async Task Eliminar()
    {
        bool confirm = await _page.DisplayAlert("Confirmar", "¿Eliminar usuario?", "Sí", "No");
        if (!confirm) return;

        var result = await _authService.EliminarUsuarioAsync(Id);
        if (result)
        {
            await _page.DisplayAlert("Eliminado", "Usuario eliminado", "OK");
            await _page.Navigation.PopAsync();
        }
        else
        {
            await _page.DisplayAlert("Error", "No se pudo eliminar", "OK");
        }
    }

    protected void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}