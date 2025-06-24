using SmartMenu.ViewModels;

namespace SmartMenu.Views;

public partial class registarProveedores : ContentPage
{
	public registarProveedores()
	{
		InitializeComponent();
        BindingContext = new RegistrarProveedorViewModel(new Services.AuthService(), this);
    }
}