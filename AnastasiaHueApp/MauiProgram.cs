using AnastasiaHueApp.Util.Alerts;
using AnastasiaHueApp.Util.Hue;
using AnastasiaHueApp.Util.Json;
using AnastasiaHueApp.ViewModels;
using Microsoft.Extensions.Logging;
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
                    .WriteTo.File(Path.Combine(FileSystem.Current.AppDataDirectory, "log.txt"), rollingInterval: RollingInterval.Day)
                    .CreateLogger());

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<IJsonRegistry, JsonRegistry>();
            builder.Services.AddSingleton<IDisplayAlertHandler, DisplayAlertHandler>();
            builder.Services.AddSingleton<HueHandler>();

            return builder.Build();
        }
    }
}
