using System.ComponentModel;
using System.Windows.Input;
using SmartMenu.Models;
using SmartMenu.Services;

namespace SmartMenu.ViewModels;

public class EditarProveedorViewModel : INotifyPropertyChanged
{
    private readonly AuthService _authService;
    private readonly Page _page;

    public event PropertyChangedEventHandler PropertyChanged;

    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Contacto { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }

    public ICommand GuardarCommand { get; }
    public ICommand EliminarCommand { get; }

    public EditarProveedorViewModel(AuthService authService, Page page, Proveedor proveedor)
    {
        _authService = authService;
        _page = page;

        Id = proveedor.Id;
        Nombre = proveedor.Nombre;
        Contacto = proveedor.Contacto;
        Telefono = proveedor.Telefono;
        Correo = proveedor.Correo;

        GuardarCommand = new Command(async () => await Guardar());
        EliminarCommand = new Command(async () => await Eliminar());
    }

    private async Task Guardar()
    {
        // Validación de campos vacíos o solo espacios
        if (string.IsNullOrWhiteSpace(Nombre) ||
            string.IsNullOrWhiteSpace(Contacto) ||
            string.IsNullOrWhiteSpace(Telefono) ||
            string.IsNullOrWhiteSpace(Correo))
        {
            await _page.DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
            return;
        }

        var actualizado = new ProveedorRequest
        {
            Nombre = Nombre.Trim(),
            Contacto = Contacto.Trim(),
            Telefono = Telefono.Trim(),
            Correo = Correo.Trim()
        };

        var result = await _authService.ActualizarProveedorAsync(Id, actualizado);
        if (result)
        {
            await _page.DisplayAlert("Éxito", "Proveedor actualizado", "OK");
            await _page.Navigation.PopAsync();
        }
        else
        {
            await _page.DisplayAlert("Error", "No se pudo actualizar", "OK");
        }
    }

    private async Task Eliminar()
    {
        bool confirm = await _page.DisplayAlert("Confirmar", "¿Eliminar proveedor?", "Sí", "No");
        if (!confirm) return;

        var result = await _authService.EliminarProveedorAsync(Id);
        if (result)
        {
            await _page.DisplayAlert("Eliminado", "Proveedor eliminado", "OK");
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