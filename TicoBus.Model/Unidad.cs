using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace TicoBus.Model
{
    public class Unidad
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Placa { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        public int AnioFabricacion { get; set; }

        [Required]
        public int CapacidadPasajeros { get; set; }

        public List<Viaje> Viajes { get; set; } = new();
    }
}