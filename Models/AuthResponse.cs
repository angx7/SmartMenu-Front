using Newtonsoft.Json;

namespace SmartMenu.Models
{
    public class AuthResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("usuario")]
        public Usuario Usuario { get; set; }
    }
}