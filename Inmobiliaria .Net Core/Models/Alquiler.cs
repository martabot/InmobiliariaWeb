using System;
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria_.Net_Core.Models {
    public class Alquiler {
        public int Id { get; set; }
        [Required,Display(Name = "Locador")]
        public Inmueble Inmueble { get; set; }
        public int IdInmueble { get; set; }
        [Required,Display(Name = "Locatario")]
        public Inquilino Inquilino { get; set; }
        public int IdInquilino { get; set; }
        [Required,Display(Name = "Fecha Inicio")]
        public String FechaInicio { get; set; }
        [Display(Name = "Fecha Fin")]
        public String FechaFin { get; set; }
        [Required,Display(Name = "Monto Total")]
        public decimal MontoTotal { get; set; }
        public Nullable<Decimal> Multa { get; set; }
    }
}
