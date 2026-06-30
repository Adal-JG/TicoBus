using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicoBus.MAUI.Interfaces;
using TicoBus.MAUI.Services;
using TicoBus.MAUI.Views;
using TicoBus.Model;

namespace TicoBus.MAUI.ViewModels
{
    public partial class MisReservasViewModel : ObservableObject
    {
        private readonly IMisViajesService _misViajesService;

        public ObservableCollection<Reserva> Reservas { get; set; } = new();

        [ObservableProperty]
        private string mensaje = string.Empty;

        [ObservableProperty]
        private bool estaCargando;

        public MisReservasViewModel(IMisViajesService misViajesService)
        {
            _misViajesService = misViajesService;
        }

        [RelayCommand]
        public async Task CargarReservas()
        {
            try
            {
                EstaCargando = true;
                Mensaje = string.Empty;
                Reservas.Clear();

                var response = await _misViajesService.ListarPorPasajeroAsync(SesionService.PasajeroId);

                if (response?.Datos == null || response.Datos.Count == 0)
                {
                    Mensaje = "No tiene reservas registradas.";
                    return;
                }

                foreach (var reserva in response.Datos)
                {
                    Reservas.Add(reserva);
                }
            }
            catch
            {
                Mensaje = "No se pudieron cargar las reservas.";
            }
            finally
            {
                EstaCargando = false;
            }
        }

        [RelayCommand]
        public async Task VerDetalle(Reserva reserva)
        {
            if (reserva == null)
                return;

            DetalleReservaViewModel.ReservaSeleccionada = reserva;
            await Shell.Current.GoToAsync(nameof(DetalleReservaPage));
        }
    }
}