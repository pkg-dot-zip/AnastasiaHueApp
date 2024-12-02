using AnastasiaHueApp.Util.Alerts;
using AnastasiaHueApp.Util.Hue;
using AnastasiaHueApp.Util.Json;
using AnastasiaHueApp.ViewModels;
using Microsoft.Extensions.Logging;
using PanCardView;
using Serilog;

namespace AnastasiaHueApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseCardsView() // From the CardsView.Maui package. See https://github.com/AndreiMisiukevich/CardView.MAUI
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Logger. See: https://www.youtube.com/watch?v=QzsQFCD5Al8. SingletonSean is my goat 🐐
            builder.Services.AddSerilog(
                new LoggerConfiguration()
                    .WriteTo.Debug()
                    .WriteTo.File(Path.Combine(FileSystem.Current.AppDataDirectory, "anastasiaLog.txt"), rollingInterval: RollingInterval.Day)
                    .CreateLogger());

            // Pages.
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<LightsPage>();

            // ViewModels.
            builder.Services.AddSingleton<MainViewModel>();

            // Util.
            builder.Services.AddSingleton<IJsonRegistry, JsonRegistry>();
            builder.Services.AddSingleton<IDisplayAlertHandler, DisplayAlertHandler>();
            builder.Services.AddSingleton<IHueHandler, HueHandler>();

            return builder.Build();
        }
    }
}
