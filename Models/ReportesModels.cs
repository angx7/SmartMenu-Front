namespace SmartMenu.Models;

public class ReporteVentaDiaria
{
    public string fecha { get; set; }
    public decimal total { get; set; }
}

public class ReportePlatilloMasVendido
{
    public string nombre { get; set; }
    public int total_vendidos { get; set; }
}

public class ReporteInsumoFaltante
{
    public string insumo { get; set; }
    public decimal stock { get; set; }
    public decimal stock_minimo { get; set; }
    public string proveedor { get; set; }
    public decimal precio { get; set; }
}