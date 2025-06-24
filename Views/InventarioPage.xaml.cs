using System.Collections.ObjectModel; // Para la lista que se puede enlazar a la UI
using System.Net.Http; // Para hacer las solicitudes HTTP
using System.Text.Json; // Para deserializar JSON
using SmartMenu.Models; // Aquí definiremos nuestro modelo de datos para los insumos

namespace SmartMenu.Views;

public partial class InventarioPage : ContentPage
{
    // ObservableCollection es ideal para listas en UI porque notifica a la UI cuando se añaden o quitan elementos
    public ObservableCollection<Insumo> Insumos { get; set; }

    public InventarioPage()
    {
        InitializeComponent();
        Insumos = new ObservableCollection<Insumo>();
        // Establecer el BindingContext para que la UI pueda acceder a la colección 'Insumos'
        this.BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarInsumos();
    }

    private async Task CargarInsumos()
    {
        
        string baseUrl = "https://da63-2a09-bac1-50c0-60-00-9f-37.ngrok-free.app"; // <--- ¡CAMBIA ESTO!
        string requestUrl = $"{baseUrl}/api/insumos"; // Combina la base con el endpoint

        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserializamos la respuesta JSON a una lista de objetos Insumo
                    var insumos = JsonSerializer.Deserialize<List<Insumo>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Esto permite que el JSON tenga nombres de propiedades con mayúsculas/minúsculas diferentes
                    });

                    Insumos.Clear(); // Limpiar la lista actual
                    if (insumos != null)
                    {
                        foreach (var insumo in insumos)
                        {
                            Insumos.Add(insumo); // Añadir cada insumo a la ObservableCollection
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudieron deserializar los insumos.", "OK");
                    }
                }
                else
                {
                    // Manejar errores HTTP (ej. 404 Not Found, 500 Internal Server Error)
                    string errorContent = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error de API", $"Error al cargar insumos: {response.StatusCode} - {errorContent}", "OK");
                }
            }
        }
        catch (HttpRequestException ex)
        {
            // Manejar errores de red o conexión
            await DisplayAlert("Error de Conexión", $"No se pudo conectar al servidor: {ex.Message}", "OK");
        }
        catch (Exception ex)
        {
            // Manejar cualquier otro error inesperado
            await DisplayAlert("Error", $"Ocurrió un error inesperado: {ex.Message}", "OK");
        }
    }
}