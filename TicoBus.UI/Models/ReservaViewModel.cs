using System.ComponentModel.DataAnnotations;

namespace TicoBus.UI.Models
{
    public class ReservaViewModel
    {
        public int ViajeId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un pasajero.")]
        public int PasajeroId { get; set; }

        [Required(ErrorMessage = "El número de asiento es requerido.")]
        [Range(1, 999, ErrorMessage = "El asiento debe ser mayor a cero.")]
        public int NumeroAsiento { get; set; }
    }
}