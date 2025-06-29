﻿using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using SmartMenu.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace SmartMenu.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl;

        public AuthService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
            BaseUrl = Environment.GetEnvironmentVariable("NGROK_URL") ?? throw new Exception("NGROK_URL no está definido en Globals/.env");
        }

        public async Task<bool> Login(string usuario, string contraseña)
        {
            try
            {
                var requestData = new { usuario, contraseña };
                var json = JsonConvert.SerializeObject(requestData);
                System.Diagnostics.Debug.WriteLine("REQUEST: " + json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{BaseUrl}/api/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine("RESPONSE: " + responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("HTTP Status: " + response.StatusCode);
                    return false;
                }

                var result = JsonConvert.DeserializeObject<AuthResponse>(responseContent);

                if (!string.IsNullOrWhiteSpace(result?.Token))
                {
                    Preferences.Set("token", result.Token);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> RegistrarUsuario(UserRequest user)
        {
            try
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrWhiteSpace(token)) return false;

                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/usuarios");
                request.Content = content;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine("🟣 Registro RESPONSE: " + responseContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("❌ Error al registrar usuario: " + ex.Message);
                return false;
            }
        }

        public async Task<string> ObtenerRolAsync(string token)
        {
            System.Diagnostics.Debug.WriteLine("Llamando a /rol con token: " + token);
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/api/rol");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.SendAsync(request);
                System.Diagnostics.Debug.WriteLine("Status code: " + response.StatusCode);

                var json = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + json);

                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine("Error en la respuesta: " + json);
                    return null;
                }

                using var doc = System.Text.Json.JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("rol", out var rolElement))
                {
                    return rolElement.GetString();
                }
                System.Diagnostics.Debug.WriteLine("No se encontró el campo 'rol' en la respuesta.");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Excepción en ObtenerRolAsync: " + ex.Message);
                return null;
            }
        }

        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            try
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrWhiteSpace(token)) return null;

                var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/api/usuarios");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine("Error al obtener usuarios: " + json);
                    return null;
                }

                return JsonConvert.DeserializeObject<List<Usuario>>(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Excepción en ObtenerUsuariosAsync: " + ex.Message);
                return null;
            }
        }

        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario)
        {
            try
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrWhiteSpace(token)) return false;

                var json = JsonConvert.SerializeObject(usuario);
                System.Diagnostics.Debug.WriteLine($"Id a actualizar: {usuario.Id}");
                System.Diagnostics.Debug.WriteLine("JSON enviado: " + json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/api/usuarios/{usuario.Id}");
                request.Content = content;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("Respuesta API: " + responseContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al actualizar usuario: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            try
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrWhiteSpace(token)) return false;

                var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/api/usuarios/{id}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al eliminar usuario: " + ex.Message);
                return false;
            }
        }

        public async Task<List<Proveedor>> ObtenerProveedoresAsync()
        {
            try
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrWhiteSpace(token)) return null;

                var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/api/proveedores");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden)
                    throw new UnauthorizedAccessException("No tienes permiso para acceder a esta ruta (403).");

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Error del servidor: {response.StatusCode}");

                return JsonConvert.DeserializeObject<List<Proveedor>>(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al obtener proveedores: " + ex.Message);
                return null;
            }
        }

        public async Task<bool> RegistrarProveedor(ProveedorRequest proveedor)
        {
            try
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrWhiteSpace(token)) return false;

                var json = JsonConvert.SerializeObject(proveedor);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/proveedores");
                request.Content = content;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine("🟣 Registro RESPONSE: " + responseContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("❌ Error al registrar usuario: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> ActualizarProveedorAsync(int id, ProveedorRequest proveedor)
        {
            try
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrWhiteSpace(token)) return false;

                var json = JsonConvert.SerializeObject(proveedor);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/api/proveedores/{id}");
                request.Content = content;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("Respuesta API: " + responseContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al actualizar proveedor: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> EliminarProveedorAsync(int id)
        {
            try
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrWhiteSpace(token)) return false;

                var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/api/proveedores/{id}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al eliminar proveedor: " + ex.Message);
                return false;
            }
        }

        public async Task<List<ReporteVentaDiaria>> ObtenerVentasDiariasAsync(int usuarioId)
        {
            var token = Preferences.Get("token", null);
            if (string.IsNullOrWhiteSpace(token)) return null;

            var body = new { usuario_id = usuarioId };
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/reportes/ventas-diarias");
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseContent);

            return JsonConvert.DeserializeObject<List<ReporteVentaDiaria>>(responseContent);
        }

        public async Task<List<ReportePlatilloMasVendido>> ObtenerPlatillosMasVendidosAsync(int usuarioId)
        {
            var token = Preferences.Get("token", null);
            if (string.IsNullOrWhiteSpace(token)) return null;

            var body = new { usuario_id = usuarioId };
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/reportes/platillos-mas-vendidos");
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseContent);

            return JsonConvert.DeserializeObject<List<ReportePlatilloMasVendido>>(responseContent);
        }

        public async Task<List<ReporteInsumoFaltante>> ObtenerInsumosFaltantesAsync(int usuarioId)
        {
            var token = Preferences.Get("token", null);
            if (string.IsNullOrWhiteSpace(token)) return null;

            var body = new { usuario_id = usuarioId };
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/reportes/insumos-faltantes");
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine("[DEBUG] Respuesta insumos faltantes: " + responseContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseContent);

            return JsonConvert.DeserializeObject<List<ReporteInsumoFaltante>>(responseContent);
        }

        public async Task<List<Insumo>> ObtenerInsumosAsync()
        {
            try
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrWhiteSpace(token))
                    return null;

                var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/api/insumos");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine("Error al obtener insumos: " + json);
                    return null;
                }

                var insumos = System.Text.Json.JsonSerializer.Deserialize<List<Insumo>>(json, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return insumos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Excepción en ObtenerInsumosAsync: " + ex.Message);
                return null;
            }
        }

        public async Task<bool> CrearInsumoAsync(Insumo insumo)
        {
            try
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrWhiteSpace(token))
                    return false;

                var url = $"{BaseUrl}/api/insumos";
                var body = new
                {
                    nombre = insumo.Nombre,
                    stock = insumo.Stock,
                    unidad = insumo.Unidad,
                    stock_minimo = insumo.StockMinimo,
                    proveedor_id = insumo.ProveedorId,
                    precio = insumo.Precio

                };
                var json = System.Text.Json.JsonSerializer.Serialize(body);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = content;
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al crear insumo: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> ActualizarInsumoAsync(Insumo insumo)
        {
            try
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrWhiteSpace(token))
                    return false;

                var url = $"{BaseUrl}/api/insumos/{insumo.Id}";
                var body = new
                {
                    nombre = insumo.Nombre,
                    stock = insumo.Stock,
                    unidad = insumo.Unidad,
                    stock_minimo = insumo.StockMinimo

                };
                var json = System.Text.Json.JsonSerializer.Serialize(body);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Put, url);
                request.Content = content;
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al actualizar insumo: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> EliminarInsumoAsync(int id)
        {
            try
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrWhiteSpace(token))
                    return false;

                var url = $"{BaseUrl}/api/insumos/{id}";
                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al eliminar insumo: " + ex.Message);
                return false;
            }
        }
    }
}
