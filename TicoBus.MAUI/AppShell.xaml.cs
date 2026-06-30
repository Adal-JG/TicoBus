using TicoBus.MAUI.Views;

namespace TicoBus.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(MisReservasPage), typeof(MisReservasPage));
        Routing.RegisterRoute(nameof(DetalleReservaPage), typeof(DetalleReservaPage));
    }
}