﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SmartMenu.Services;
using SmartMenu.Views;

namespace SmartMenu.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private readonly Page _page;

        private string _usuario;
        private string _contrasena;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Usuario
        {
            get => _usuario;
            set { _usuario = value; OnPropertyChanged(); }
        }

        public string Contrasena
        {
            get => _contrasena;
            set { _contrasena = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel(AuthService authService, Page page)
        {
            _authService = authService;
            _page = page;

            LoginCommand = new Command(async () => await Login());
            RegisterCommand = new Command(async () => await NavigateToRegister());
        }

        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Contrasena))
            {
                await _page.DisplayAlert("Error", "Por favor ingresa usuario y contraseña", "OK");
                return;
            }

            var isAuthenticated = await _authService.Login(Usuario, Contrasena);

            if (isAuthenticated)
            {
                var token = Preferences.Get("token", null);
                if (token != null)
                {
                    // Decodifica el usuario_id y guárdalo en Preferences
                    try
                    {
                        var usuarioId = JwtHelper.GetUsuarioId(token);
                        if (usuarioId.HasValue)
                        {
                            Preferences.Set("usuario_id", usuarioId.Value);
                            System.Diagnostics.Debug.WriteLine($"[DEBUG] usuario_id guardado: {usuarioId.Value}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("[ERROR] usuario_id no encontrado en el token");
                        }

                        // (Opcional) Muestra el payload para depuración
                        var payload = JwtHelper.DecodePayload(token);
                        foreach (var kv in payload)
                        {
                            System.Diagnostics.Debug.WriteLine($"[JWT] {kv.Key} = {kv.Value}");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error decodificando usuario_id: " + ex.Message);
                    }
                    var rol = await _authService.ObtenerRolAsync(token);
                    if (!string.IsNullOrEmpty(rol))
                    {
                        System.Diagnostics.Debug.WriteLine("Tu rol es " + rol);
                        if (rol.Equals("administrador"))
                        {
                            Application.Current.MainPage = new NavigationPage(new AdminView());
                        }
                        else if (rol.Equals("mesero"))
                        {
                            Application.Current.MainPage = new NavigationPage(new MeseroPage());
                        }
                        else if (rol.Equals("cocinero"))
                        {
                            Application.Current.MainPage = new NavigationPage(new CocinaPage());
                        }
                        else
                        {
                            Application.Current.MainPage = new NavigationPage(new AppShell());
                        }
                    }
                }
                else
                {
                    await _page.DisplayAlert("Error", "No se pudo obtener el token", "OK");
                }
            }
            else
            {
                await _page.DisplayAlert("Error", "Credenciales incorrectas", "OK");
            }
        }

        private async Task NavigateToRegister()
        {
            // Navegación opcional a registro
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}