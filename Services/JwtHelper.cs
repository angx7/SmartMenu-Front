using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SmartMenu.Services
{
    public static class JwtHelper
    {
        /// <summary>
        /// Decodifica el payload de un JWT y lo regresa como diccionario.
        /// </summary>
        public static Dictionary<string, object> DecodePayload(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 3)
                throw new ArgumentException("Token no válido");

            var payload = parts[1];
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            var bytes = Convert.FromBase64String(payload);
            var json = Encoding.UTF8.GetString(bytes);

            // Usar System.Text.Json para deserializar a Dictionary<string, object>
            return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        }

        /// <summary>
        /// Obtiene el rol_id del token JWT.
        /// </summary>
        public static int GetRolId(string token)
        {
            var payload = DecodePayload(token);
            if (payload.TryGetValue("rol_id", out var rolId))
            {
                if (rolId is JsonElement jsonElement && jsonElement.TryGetInt32(out var intRolId))
                    return intRolId;
                if (int.TryParse(rolId.ToString(), out int intRolId2))
                    return intRolId2;
            }
            throw new Exception("El token no contiene rol_id");
        }

        /// <summary>
        /// Obtiene el usuario_id del token JWT.
        /// </summary>
        public static int? GetUsuarioId(string token)
        {
            var payload = DecodePayload(token);
            if (payload.TryGetValue("id", out var usuarioId))
            {
                if (usuarioId is JsonElement jsonElement && jsonElement.TryGetInt32(out var intUsuarioId))
                    return intUsuarioId;
                if (int.TryParse(usuarioId.ToString(), out int intUsuarioId2))
                    return intUsuarioId2;
            }
            // Si no existe, regresa null
            return null;
        }
    }
}