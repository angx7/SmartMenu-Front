using System.Text.Json.Serialization; // Asegúrate de que esta línea esté presente

namespace SmartMenu.Models
{
    public class Insumo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Stock { get; set; } // Confirmado como string debido al error anterior
        public string Unidad { get; set; }

        [JsonPropertyName("stock_minimo")] // ¡CRUCIAL! Mapea el JSON "stock_minimo"
        public string StockMinimo { get; set; } // ¡CRUCIAL! Nombre de la propiedad en C# (PascalCase)
    }
}