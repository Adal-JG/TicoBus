using TicoBus.MAUI.ViewModels;

namespace TicoBus.MAUI.Views;

public partial class DetalleReservaPage : ContentPage
{
    public DetalleReservaPage()
    {
        InitializeComponent();

        BindingContext = new DetalleReservaViewModel();
    }
}