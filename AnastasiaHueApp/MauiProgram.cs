using AnastasiaHueApp.Util.Alerts;
using AnastasiaHueApp.Util.Hue;
using AnastasiaHueApp.Util.Json;
using AnastasiaHueApp.Util.Preferences;
using AnastasiaHueApp.Util.Shell;
using AnastasiaHueApp.ViewModels;
using CommunityToolkit.Maui;
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
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("JupiteroidItalic.ttf", "JupiteroidItalic");
                    fonts.AddFont("JupiteroidLight.ttf", "JupiteroidLight");
                    fonts.AddFont("JupiteroidLightItalic.ttf", "JupiteroidLightItalic");
                    fonts.AddFont("JupiteroidRegular.ttf", "Jupiteroid");
                    fonts.AddFont("JupiteroidBold.ttf", "JupiteroidBold");
                    fonts.AddFont("JupiteroidBoldItalic.ttf", "JupiteroidBoldItalic");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Logger. See: https://www.youtube.com/watch?v=QzsQFCD5Al8. SingletonSean is my goat 🐐
            // TODO: Found out if files (and if so, where?!) are created on Android.
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
            builder.Services.AddSingleton<IHttpClientContainer, HueHttpClientContainer>();
            builder.Services.AddSingleton<IPreferences>(Preferences.Default);
            builder.Services.AddSingleton<IPreferencesHandler, PreferencesHandler>();
            builder.Services.AddSingleton<IShellContainer, ShellContainer>();

            return builder.Build();
        }
    }
}
