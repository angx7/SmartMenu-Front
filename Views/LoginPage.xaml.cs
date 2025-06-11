using SmartMenu.ViewModels;
using SmartMenu.Services;

namespace SmartMenu.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		BindingContext = new LoginViewModel(new AuthService(), this);
	}

	public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}