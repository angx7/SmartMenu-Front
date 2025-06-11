using SmartMenu.ViewModels;

namespace SmartMenu.Views;

public partial class AddUserPage : ContentPage
{
    public AddUserPage()
    {
        InitializeComponent();
        BindingContext = new AddUserViewModel(new Services.AuthService(), this);
    }
}
