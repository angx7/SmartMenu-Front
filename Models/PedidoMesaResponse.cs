using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenu.Models
{
    internal class PedidoMesaResponse
    {
        public int pedido_id { get; set; }
        public bool nuevo { get; set; }
        public List<DetallePedido> detalles { get; set; }
        public double total { get; set; }
    }
}
