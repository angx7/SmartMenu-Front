using System.Text.Json.Serialization; // Asegúrate de que esta línea esté presente

namespace SmartMenu.Models
{
       public class Insumo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Stock { get; set; }
        public string Unidad { get; set; }

        [JsonPropertyName("stock_minimo")]
        public string StockMinimo { get; set; }

        public int ProveedorId { get; set; }
        public decimal Precio { get; set; }
    }
}