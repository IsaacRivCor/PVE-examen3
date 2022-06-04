using System.ComponentModel.DataAnnotations;

namespace Examen3.Models
{
    public class Entrada
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name ="Nombre de la entrada")]
        public string? nombre { get; set; }
        public double precio { get; set; }
        public string? urlImagen { get; set; }
    }
}