using System.ComponentModel.DataAnnotations;

namespace TicoBus.UI.Models
{
    public class RutaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la ruta es requerido.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El origen es requerido.")]
        [StringLength(100)]
        public string Origen { get; set; } = string.Empty;

        [Required(ErrorMessage = "El destino es requerido.")]
        [StringLength(100)]
        public string Destino { get; set; } = string.Empty;

        [Required(ErrorMessage = "La duración estimada es requerida.")]
        [RegularExpression(@"^([0-9]{1,2}):([0-5][0-9])$", ErrorMessage = "Use el formato hh:mm. Ejemplo: 02:30")]
        public string DuracionEstimada { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio base es requerido.")]
        [Range(1, 9999999, ErrorMessage = "El precio base debe ser mayor a cero.")]
        public decimal PrecioBase { get; set; }
    }
}