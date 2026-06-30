using System.ComponentModel.DataAnnotations;

namespace TicoBus.UI.Models
{
    public class PerfilViewModel
    {
        public int Id { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string NombreCompleto { get; set; } = string.Empty;

        public string Rol { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        public string Correo { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string? ClaveActual { get; set; }

        [DataType(DataType.Password)]
        public string? NuevaClave { get; set; }

        [DataType(DataType.Password)]
        [Compare("NuevaClave", ErrorMessage = "Las claves no coinciden.")]
        public string? ConfirmarClave { get; set; }
    }
}