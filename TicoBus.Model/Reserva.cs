using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicoBus.Model
{
    public class Reserva
    {
        public int Id { get; set; }

        public int ViajeId { get; set; }
        public Viaje? Viaje { get; set; }

        public int PasajeroId { get; set; }
        public Pasajero? Pasajero { get; set; }

        [Required]
        public int NumeroAsiento { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoPagado { get; set; }

        public bool Activa { get; set; } = true;

        public DateTime FechaReserva { get; set; } = DateTime.Now;
    }
}