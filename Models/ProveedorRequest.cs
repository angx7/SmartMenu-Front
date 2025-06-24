using Newtonsoft.Json;

namespace SmartMenu.Models;

public class ProveedorRequest
{
    [JsonProperty("nombre")]
    public string Nombre { get; set; }

    [JsonProperty("contacto")]
    public string Contacto { get; set; }

    [JsonProperty("telefono")]
    public string Telefono { get; set; }

    [JsonProperty("correo")]
    public string Correo { get; set; }
}
