using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria_.Net_Core.Models {
    public class Propietario {
        public int IdPropietario { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Dni { get; set; }
        public string Telefono { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Clave { get; set; }
    }
}
