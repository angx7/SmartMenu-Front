using SmartMenu.Services;
using SmartMenu.ViewModels;
using SmartMenu.Models;

namespace SmartMenu.Views;

public partial class EditarProveedorPage : ContentPage
{
    public EditarProveedorPage(Proveedor proveedor)
    {
        InitializeComponent();
        BindingContext = new EditarProveedorViewModel(new AuthService(), this, proveedor);
    }
}