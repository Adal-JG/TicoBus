using System.ComponentModel.DataAnnotations;

namespace TicoBus.UI.Models
{
    public class UnidadViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La placa es requerida.")]
        [StringLength(20)]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es requerido.")]
        [StringLength(100)]
        public string Modelo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El año de fabricación es requerido.")]
        [Range(1980, 2100, ErrorMessage = "Ingrese un año válido.")]
        public int AnioFabricacion { get; set; }

        [Required(ErrorMessage = "La capacidad de pasajeros es requerida.")]
        [Range(1, 100, ErrorMessage = "La capacidad debe ser mayor a cero.")]
        public int CapacidadPasajeros { get; set; }
    }
}