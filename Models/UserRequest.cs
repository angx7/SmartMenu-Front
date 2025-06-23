using Newtonsoft.Json;

namespace SmartMenu.Models;

public class UserRequest
{
    [JsonProperty("nombre")]
    public string Nombre { get; set; }

    [JsonProperty("usuario")]
    public string Usuario { get; set; }

    [JsonProperty("contraseña")]
    public string Contraseña { get; set; }

    [JsonProperty("rol_id")]
    public int Rol_Id { get; set; }

    }
