using System;
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria_.Net_Core.Models {
    public class Pago {
        public int Id { get; set; }
        public Alquiler Alquiler { get; set; }
        public int IdAlquiler { get; set; }
        [Required,Display(Name ="Número de pago")]
        public int NroPago { get; set; }
        [Required]
        public String Fecha { get; set; }
        [Required]
        public decimal Importe { get; set; }
    }
}

