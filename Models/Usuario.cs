﻿using Newtonsoft.Json;

namespace SmartMenu.Models
{
    public class Usuario
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("usuario")]
        public string NombreUsuario { get; set; }

        [JsonProperty("rol")]
        public string Rol { get; set; }

        [JsonProperty("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }
    }
}