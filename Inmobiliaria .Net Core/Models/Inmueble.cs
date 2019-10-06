using System;
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria_.Net_Core.Models {
    public class Inmueble {
        public int Id { get; set; }
        [Required]
        public String Direccion { get; set; }
        public Propietario Propietario { get; set; }
        public int IdPropietario { get; set; }
        public int Disponible { get; set; }
        [Required]
        public int Ambientes { get; set; }
        [Required]
        public Decimal Precio { get; set; }
        [Required]
        public string Categoria { get; set; }
        [Required]
        public string Uso { get; set; }
        [Required]
        public string Transaccion { get; set; }
    }
}
