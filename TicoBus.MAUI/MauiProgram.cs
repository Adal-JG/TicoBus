using Microsoft.Extensions.Logging;
using TicoBus.MAUI.Interfaces;
using TicoBus.MAUI.Services;
using TicoBus.MAUI.ViewModels;
using TicoBus.MAUI.Views;


namespace TicoBus.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IMisViajesService, MisViajesService>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<MisReservasViewModel>();
            builder.Services.AddTransient<DetalleReservaViewModel>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<MisReservasPage>();
            builder.Services.AddTransient<DetalleReservaPage>();
            builder.Services.AddSingleton<IPasajeroService, PasajeroService>();
            return builder.Build();
        }
    }
}
