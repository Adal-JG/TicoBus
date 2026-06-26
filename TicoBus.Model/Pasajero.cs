using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace TicoBus.Model
{
    public class Pasajero
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Identificacion { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }

        public List<Reserva> Reservas { get; set; } = new();
    }
}