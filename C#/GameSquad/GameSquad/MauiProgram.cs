using GameSquad.Service;
using GameSquad.ViewModel;
using Microsoft.Extensions.Logging;

namespace GameSquad
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

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<AuthViewModel>();
            builder.Services.AddSingleton<RegisterViewModel>(); 

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
