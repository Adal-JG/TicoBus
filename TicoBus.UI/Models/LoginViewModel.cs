using System.ComponentModel.DataAnnotations;

namespace TicoBus.UI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario es requerido.")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La clave es requerida.")]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = string.Empty;
    }
}