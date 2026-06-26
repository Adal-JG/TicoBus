using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace TicoBus.Model
{
    public class Viaje
    {
        public int Id { get; set; }

        public int RutaId { get; set; }
        public Ruta? Ruta { get; set; }

        public int UnidadId { get; set; }
        public Unidad? Unidad { get; set; }

        public int ChoferId { get; set; }
        public Chofer? Chofer { get; set; }

        [Required]
        public DateTime FechaHoraSalida { get; set; }

        [Required]
        public DateTime FechaHoraLlegadaEstimada { get; set; }

        [Required]
        public EstadoViaje Estado { get; set; } = EstadoViaje.Programado;

        [StringLength(500)]
        public string? MotivoCancelacion { get; set; }

        public List<Reserva> Reservas { get; set; } = new();
    }
}