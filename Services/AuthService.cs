using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using SmartMenu.Models;
using Microsoft.Maui.Storage;

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

    }
}