using System.ComponentModel.DataAnnotations;

namespace Examen3.Models

{
    public class Bebida
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name ="Bebida")]
        public string? nombre { get; set; }
        [Required]
        [Display(Name ="Tamaño (ml)")]
        public int tmaño { get; set; }
        public double precio { get; set; }
        [Display(Name ="Imagen de la Bebida")]
        public string? urlImagen { get; set; }

    }
}