using SmartMenu.Models;
using SmartMenu.ViewModels;

namespace SmartMenu.Views;

public partial class EditarInsumoPage : ContentPage
{
    public EditarInsumoPage(Insumo insumo, InventarioViewModel inventarioViewModel)
    {
        InitializeComponent();
        BindingContext = new EditarInsumoViewModel(insumo, Navigation, inventarioViewModel);
    }
}