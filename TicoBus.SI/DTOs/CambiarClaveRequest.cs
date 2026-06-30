namespace TicoBus.SI.DTOs
{
    public class CambiarClaveRequest
    {
        public string NombreUsuario { get; set; } = string.Empty;

        public string ClaveActual { get; set; } = string.Empty;

        public string NuevaClave { get; set; } = string.Empty;
    }
}