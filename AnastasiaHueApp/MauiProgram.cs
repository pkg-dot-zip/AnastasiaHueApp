using AnastasiaHueApp.Util.Alerts;
using AnastasiaHueApp.Util.Hue;
using AnastasiaHueApp.Util.Json;
using AnastasiaHueApp.Util.Preferences;
using AnastasiaHueApp.Util.Shell;
using AnastasiaHueApp.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
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
            builder.Services.AddSingleton<IStorageHandler, StorageHandler>();
            builder.Services.AddSingleton<IShellContainer, ShellContainer>();


            // SEE: https://blog.verslu.is/maui/full-screen-disable-minimize-maximize-for-net-maui-windows-apps/
#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                // Make sure to add "using Microsoft.Maui.LifecycleEvents;" in the top of the file 
                events.AddWindows(windowsLifecycleBuilder =>
                {
                    windowsLifecycleBuilder.OnWindowCreated(window =>
                    {
                        window.ExtendsContentIntoTitleBar = false;
                        var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                        switch (appWindow.Presenter)
                        {
                            case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                                // overlappedPresenter.SetBorderAndTitleBar(false, false);
                                overlappedPresenter.Maximize();
                                overlappedPresenter.IsMaximizable = false; // Disables maximize button.
                                break;
                        }
                    });
                });
            });
#endif

            return builder.Build();
        }
    }
}
