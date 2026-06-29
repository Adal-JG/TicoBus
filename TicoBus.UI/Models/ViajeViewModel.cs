using System.ComponentModel.DataAnnotations;

namespace TicoBus.UI.Models
{
    public class ViajeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La ruta es requerida.")]
        public int RutaId { get; set; }

        [Required(ErrorMessage = "La unidad es requerida.")]
        public int UnidadId { get; set; }

        [Required(ErrorMessage = "El chofer es requerido.")]
        public int ChoferId { get; set; }

        [Required(ErrorMessage = "La fecha y hora de salida es requerida.")]
        public DateTime FechaHoraSalida { get; set; }

        [Required(ErrorMessage = "La fecha y hora estimada de llegada es requerida.")]
        public DateTime FechaHoraLlegadaEstimada { get; set; }

        public string? MotivoCancelacion { get; set; }
    }
}