using System.Threading.Tasks;

namespace SmartMenu.Views;

public partial class CocinaPage : ContentPage
{
	public CocinaPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventarioPage());
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PedidosPage());
    }
}