using SmartMenu.Models;
using SmartMenu.ViewModels;
using SmartMenu.Services;

namespace SmartMenu.Views;

public partial class EditarUsuarioPage : ContentPage
{
    public EditarUsuarioPage(Usuario usuario)
    {
        InitializeComponent();
        BindingContext = new EditarUsuarioViewModel(new AuthService(), this, usuario);
    }
}