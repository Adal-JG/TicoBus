namespace TicoBus.UI.ApiClients
{
    public class AuthApiClient : ApiClientBase
    {
        public AuthApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
        }

        public async Task<LoginResponse?> Login(string nombreUsuario, string clave)
        {
            var request = new LoginRequest
            {
                NombreUsuario = nombreUsuario,
                Clave = clave
            };

            var response = await PostAsync<LoginResponse>("Auth/login", request);

            return response?.Datos ?? new LoginResponse
            {
                Exitoso = response?.Exitoso ?? false,
                Mensaje = response?.Mensaje ?? "No se pudo conectar con el API."
            };
        }
    }
}