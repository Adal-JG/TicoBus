using TicoBus.MAUI.Services;
using TicoBus.MAUI.ViewModels;

namespace TicoBus.MAUI.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();

        BindingContext = new LoginViewModel(
            new AuthService(),
            new PasajeroService()
        );
    }
}