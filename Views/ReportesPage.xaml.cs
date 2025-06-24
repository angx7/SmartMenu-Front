using SmartMenu.Models;
using SmartMenu.Services;
using System.Collections.Generic;
using System.Linq;

namespace SmartMenu.Views;

public partial class ReportesPage : ContentPage
{
    private readonly AuthService _authService = new();

    public ReportesPage()
    {
        InitializeComponent();
        lblTituloReporte.IsVisible = false;
        cvReporte.IsVisible = false;
    }

    private void MostrarReporte(string titulo, IEnumerable<string> lineas)
    {
        lblTituloReporte.Text = titulo;
        lblTituloReporte.IsVisible = true;
        cvReporte.ItemsSource = lineas.ToList();
        cvReporte.IsVisible = true;
    }

    private void LimpiarReporte(string mensaje = null)
    {
        lblTituloReporte.IsVisible = false;
        cvReporte.IsVisible = false;
        if (!string.IsNullOrWhiteSpace(mensaje))
            DisplayAlert("Aviso", mensaje, "OK");
    }

    private async void OnVentasDiariasClicked(object sender, EventArgs e)
    {
        int usuarioId = Preferences.Get("usuario_id", 0);
        if (usuarioId == 0)
        {
            LimpiarReporte("No se encontró el usuario_id");
            return;
        }

        try
        {
            var ventas = await _authService.ObtenerVentasDiariasAsync(usuarioId);
            if (ventas == null || ventas.Count == 0)
            {
                LimpiarReporte("No hay ventas diarias.");
                return;
            }

            var lineas = ventas.Select(v =>
            {
                string fechaFormateada;
                if (DateTime.TryParse(v.fecha, out var fecha))
                    fechaFormateada = fecha.ToString("dd/MM/yyyy");
                else
                    fechaFormateada = v.fecha; // fallback si no se puede parsear

                return $"Fecha: {fechaFormateada}\nTotal: ${v.total:F2}";
            });
            MostrarReporte("Ventas diarias", lineas);
        }
        catch (Exception ex)
        {
            LimpiarReporte("Error: " + ex.Message);
        }
    }

    private async void OnPlatillosMasVendidosClicked(object sender, EventArgs e)
    {
        int usuarioId = Preferences.Get("usuario_id", 0);
        if (usuarioId == 0)
        {
            LimpiarReporte("No se encontró el usuario_id");
            return;
        }

        try
        {
            var platillos = await _authService.ObtenerPlatillosMasVendidosAsync(usuarioId);
            if (platillos == null || platillos.Count == 0)
            {
                LimpiarReporte("No hay platillos más vendidos.");
                return;
            }

            var lineas = platillos.Select(p => $"{p.nombre}: {p.total_vendidos} vendidos");
            MostrarReporte("Platillos más vendidos", lineas);
        }
        catch (Exception ex)
        {
            LimpiarReporte("Error: " + ex.Message);
        }
    }

    private async void OnInsumosFaltantesClicked(object sender, EventArgs e)
    {
        int usuarioId = Preferences.Get("usuario_id", 0);
        if (usuarioId == 0)
        {
            LimpiarReporte("No se encontró el usuario_id");
            return;
        }

        try
        {
            var insumos = await _authService.ObtenerInsumosFaltantesAsync(usuarioId);
            if (insumos == null || insumos.Count == 0)
            {
                LimpiarReporte("No hay insumos faltantes.");
                return;
            }

            var lineas = insumos.Select(i =>
                $"Insumo: {i.insumo}\nStock: {i.stock}/{i.stock_minimo}\nProveedor: {i.proveedor} (${i.precio:F2})"
            );
            MostrarReporte("Insumos faltantes", lineas);
        }
        catch (Exception ex)
        {
            LimpiarReporte("Error: " + ex.Message);
        }
    }
}