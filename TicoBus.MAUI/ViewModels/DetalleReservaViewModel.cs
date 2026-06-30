using CommunityToolkit.Mvvm.ComponentModel;
using TicoBus.Model;

namespace TicoBus.MAUI.ViewModels
{
    public partial class DetalleReservaViewModel : ObservableObject
    {
        public static Reserva? ReservaSeleccionada { get; set; }

        [ObservableProperty]
        private Reserva? reserva;

        public DetalleReservaViewModel()
        {
            Reserva = ReservaSeleccionada;
        }
    }
}