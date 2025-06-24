using System.Collections.Generic;

namespace SmartMenu.Models
{
    public class ComandaPedido
    {
        public int pedido_id { get; set; }
        public int mesa_id { get; set; }
        public string estado { get; set; }
        public string fecha { get; set; }
        public string mesero { get; set; }
        public List<ComandaDetalle> comanda { get; set; }
    }

    public class ComandaDetalle
    {
        public int pedido_id { get; set; }
        public string platillo { get; set; }
        public int cantidad { get; set; }
    }
}