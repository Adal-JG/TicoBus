namespace TicoBus.MAUI.DTOs
{
    public class ApiResponse<T>
    {
        public bool Exitoso { get; set; }
        public string? Mensaje { get; set; }
        public T? Datos { get; set; }
    }
}