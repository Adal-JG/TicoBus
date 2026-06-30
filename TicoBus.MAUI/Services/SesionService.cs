namespace TicoBus.MAUI.Services
{
    public static class SesionService
    {
        public static int UsuarioId { get; set; }
        public static int PasajeroId { get; set; }
        public static string Nombre { get; set; } = string.Empty;
        public static string Rol { get; set; } = string.Empty;

        public static void CerrarSesion()
        {
            UsuarioId = 0;
            PasajeroId = 0;
            Nombre = string.Empty;
            Rol = string.Empty;
        }
    }
}