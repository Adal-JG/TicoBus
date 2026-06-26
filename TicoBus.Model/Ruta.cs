using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicoBus.Model
{
    public class Ruta
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Origen { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Destino { get; set; } = string.Empty;

        [Required]
        public TimeSpan DuracionEstimada { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioBase { get; set; }

        public List<Viaje> Viajes { get; set; } = new();
    }
}