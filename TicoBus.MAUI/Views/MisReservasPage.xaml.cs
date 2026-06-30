using TicoBus.MAUI.Services;
using TicoBus.MAUI.ViewModels;

namespace TicoBus.MAUI.Views;

public partial class MisReservasPage : ContentPage
{
    private readonly MisReservasViewModel vm;

    public MisReservasPage()
    {
        InitializeComponent();

        vm = new MisReservasViewModel(new MisViajesService());

        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await vm.CargarReservasCommand.ExecuteAsync(null);
    }
}