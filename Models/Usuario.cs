using Newtonsoft.Json;

namespace SmartMenu.Models
{
    public class Usuario
    {
        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("usuario")]
        public string NombreUsuario { get; set; }

        [JsonProperty("contraseña")]
        public string Contraseña { get; set; }

        [JsonProperty("rol_id")]
        public int Rol_Id { get; set; }
    }
}