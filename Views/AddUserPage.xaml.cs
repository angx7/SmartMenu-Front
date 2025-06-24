using SmartMenu.ViewModels;
using System.Collections.ObjectModel;

namespace SmartMenu.Views;

public partial class AddUserPage : ContentPage
{
    public AddUserPage()
    {
        InitializeComponent();
        BindingContext = new AddUserViewModel(new Services.AuthService(), this);
    }
}
