using SmartMenu.ViewModels;

namespace SmartMenu.Views;

public partial class AgregarInsumoPage : ContentPage
{
    public AgregarInsumoPage()
    {
        InitializeComponent();
        BindingContext = new AgregarInsumoViewModel(Navigation);
    }
}