using TicoBus.Model;

namespace TicoBus.UI.ApiClients
{
    public class PerfilApiClient : ApiClientBase
    {
        public PerfilApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<Usuario?> ObtenerUsuario(int id)
        {
            var response = await GetAsync<Usuario>($"Perfil/{id}");
            return response?.Datos;
        }

        public async Task<ApiResponse<Usuario>?> ActualizarCorreo(int id, string correo)
        {
            return await PutAsync<Usuario>($"Perfil/{id}/correo", new
            {
                Correo = correo
            });
        }

        public async Task<ApiResponse<object>?> CambiarClave(string nombreUsuario, string claveActual, string nuevaClave)
        {
            return await PostAsync<object>("Auth/cambiar-clave", new
            {
                NombreUsuario = nombreUsuario,
                ClaveActual = claveActual,
                NuevaClave = nuevaClave
            });
        }
    }
}