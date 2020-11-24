using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Sistema.Entidades.Usuarios;

namespace Sistema.Entidades.Ventas
{
    public class Venta
    {
        public int idventa { get; set; }
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

        public ICollection<DetalleVenta> detalles { get; set; }
        public Usuario usuario { get; set; }
        public Persona persona { get; set; }
    }
}
