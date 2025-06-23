using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SmartMenu.Services
{
    public static class JwtHelper
    {
        public static Dictionary<string, object> DecodePayload(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 3)
                throw new ArgumentException("Token no válido");

            var payload = parts[1];
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            var bytes = Convert.FromBase64String(payload);
            var json = Encoding.UTF8.GetString(bytes);

            return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        }

        public static int GetRolId(string token)
        {
            var payload = DecodePayload(token);
            if (payload.TryGetValue("rol_id", out var rolId))
            {
                return Convert.ToInt32(rolId);
            }

            throw new Exception("El token no contiene rol_id");
        }
    }
}
