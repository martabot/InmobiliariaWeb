using System;
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria_.Net_Core.Models {
    public class Inquilino {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Dni { get; set; }
        public string Telefono { get; set; }
        [Required, EmailAddress]
        public string Mail { get; set; }
        [Required][Display(Name = "Direccion de Trabajo")]
        public String DireccionTrabajo { get; set; }
        [Required][Display(Name = "Dni garante")]
        public String DniGarante { get; set; }
        [Required][Display(Name = "Tel. Garante")]
        public String TelGarante { get; set; }
    }
}
