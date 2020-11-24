using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Web.Models.Ventas.Venta
{
    public class CrearViewModel
    {
        //Propiedades maestro
        [Required]
        public int idcliente { get; set; }
        [Required]
        public int idusuario { get; set; }
        [Required]
        public int dias { get; set; }
        [Required]
        public decimal procen_interes { get; set; }
        [Required]
        public decimal intereses { get; set; }
        [Required]
        public decimal total_venta { get; set; }
        [Required]
        public decimal total_deuda { get; set; }
        [Required]
        public string tipo_interes { get; set; }
        [Required]
        public string estado { get; set; }
        //Propiedades detalle
        [Required]
        public List<DetalleViewModel> detalles { get; set; }

    }
}
