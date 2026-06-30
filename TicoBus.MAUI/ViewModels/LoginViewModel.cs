using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicoBus.MAUI.DTOs;
using TicoBus.MAUI.Interfaces;
using TicoBus.MAUI.Services;
using TicoBus.MAUI.Views;

namespace TicoBus.MAUI.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string nombreUsuario = string.Empty;
        private readonly IPasajeroService _pasajeroService;
        [ObservableProperty]
        private string clave = string.Empty;

        [ObservableProperty]
        private string mensajeError = string.Empty;

        [ObservableProperty]
        private bool estaCargando;

        public LoginViewModel(IAuthService authService, IPasajeroService pasajeroService)
        {
            _authService = authService;
            _pasajeroService = pasajeroService;
        }

        [RelayCommand]
        public async Task Login()
        {
            MensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(NombreUsuario) || string.IsNullOrWhiteSpace(Clave))
            {
                MensajeError = "Debe ingresar usuario y clave.";
                return;
            }

            try
            {
                EstaCargando = true;

                var request = new LoginRequest
                {
                    NombreUsuario = NombreUsuario,
                    Clave = Clave
                };

                var response = await _authService.LoginAsync(request);

                if (response == null || !response.Exitoso || response.Datos == null)
                {
                    MensajeError = response?.Mensaje ?? "No se pudo iniciar sesión.";
                    return;
                }

                if (response.Datos.Rol != "Pasajero")
                {
                    MensajeError = "Esta aplicación es solo para pasajeros.";
                    return;
                }

                SesionService.UsuarioId = response.Datos.UsuarioId;
                var pasajerosResponse = await _pasajeroService.ListarAsync();
                var pasajero = pasajerosResponse?.Datos?
                    .FirstOrDefault(p => p.UsuarioId == SesionService.UsuarioId);

                if (pasajero == null)
                {
                    MensajeError = "No se encontró el pasajero asociado al usuario.";
                    return;
                }

                SesionService.PasajeroId = pasajero.Id;
                SesionService.Nombre = response.Datos.Nombre ?? "";
                SesionService.Rol = response.Datos.Rol ?? "";

                await Shell.Current.GoToAsync($"//{nameof(MisReservasPage)}");
            }
            catch (Exception ex)
            {
                MensajeError = ex.Message;
            }
            finally
            {
                EstaCargando = false;
            }
        }
    }
}