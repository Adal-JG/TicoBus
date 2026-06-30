namespace TicoBus.MAUI.DTOs
{
    public class LoginResponse
    {
        public bool Exitoso { get; set; }
        public string? Mensaje { get; set; }
        public int UsuarioId { get; set; }
        public string? Nombre { get; set; }
        public string? Rol { get; set; }
    }
}