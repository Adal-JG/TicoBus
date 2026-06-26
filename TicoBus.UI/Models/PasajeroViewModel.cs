using System.ComponentModel.DataAnnotations;

namespace TicoBus.UI.Models
{
    public class PasajeroViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La identificación es requerida.")]
        [StringLength(30)]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son requeridos.")]
        [StringLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;
    }
}