using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Models.Ventas.Venta
{
    public class VentaViewModel
    {
        public int idventa { get; set; }
        public int idcliente { get; set; }
        public string cliente { get; set; }
        public string num_documento { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public int idusuario { get; set; }
        public string usuario { get; set; }
        public int dias { get; set; }
        public decimal procen_interes { get; set; }
        public decimal intereses { get; set; }
        public decimal total_venta { get; set; }
        public decimal total_deuda { get; set; }
        public string tipo_interes { get; set; }
        public string estado { get; set; }
    }
}
