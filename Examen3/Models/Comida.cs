using System.ComponentModel.DataAnnotations;

namespace Examen3.Models
{
    public class Comida
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name ="Nombre del platillo")]
        public string? nombre { get; set; }
        public double precio { get; set; }
        public string? urlImagen { get; set; }
    }
}