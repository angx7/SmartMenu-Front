using SmartMenu.Models;
using SmartMenu.Services;
using Newtonsoft.Json;
using System.Net;

namespace SmartMenu.Views;
public partial class usuariosPage : ContentPage
{
    private readonly AuthService _authService = new();
    private List<Usuario> _todosLosUsuarios = new();

    public usuariosPage()
    {
        InitializeComponent();
        CargarUsuarios();
    }

    private async void CargarUsuarios()
    {
        try
        {
            var usuarios = await _authService.ObtenerUsuariosAsync();
            if (usuarios == null)
            {
                await DisplayAlert("Error", "No se pudieron obtener los usuarios.", "OK");
                return;
            }

            _todosLosUsuarios = usuarios;

            MostrarUsuariosFiltrados();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }

    private void MostrarUsuariosFiltrados()
    {
        usuariosLayout.Children.Clear();

        int rolId = rolPicker.SelectedIndex;
        IEnumerable<Usuario> filtrados = _todosLosUsuarios;

        if (rolId > 0) // 0 = Todos, 1 = Administrador, 2 = Mesero, 3 = Cocinero
        {
            string rolBuscado = rolId switch
            {
                1 => "administrador",
                2 => "mesero",
                3 => "cocinero",
                _ => "todos"
            };
            filtrados = _todosLosUsuarios.Where(u => u.Rol_Id == rolId);
        }

        if (!filtrados.Any())
        {
            usuariosLayout.Children.Add(new Label { Text = "No se encontraron usuarios.", TextColor = Colors.White });
            return;
        }

        foreach (var usuario in filtrados)
        {
            var boton = new Button
            {
                Text = $"{usuario.Rol_Id} - {usuario.Nombre}",
                BackgroundColor = Colors.Red,
                TextColor = Colors.White,
                CornerRadius = 20
            };

            boton.Clicked += (s, e) =>
            {
                DisplayAlert("Sin teléfono", "Este usuario no tiene número registrado.", "OK");
            };

            usuariosLayout.Children.Add(boton);
        }
    }

    private void OnRolPickerChanged(object sender, EventArgs e)
    {
        MostrarUsuariosFiltrados();
    }
}