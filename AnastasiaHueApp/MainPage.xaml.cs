using AnastasiaHueApp.Util.Hue;
using AnastasiaHueApp.Util.Shell;
using AnastasiaHueApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp
{
    public partial class MainPage : ContentPage
    {
        private static bool _forcedNavigationAlready = false;

        public MainPage(MainViewModel viewModel, ILogger<LightsPage> logger, IHueHandler hueHandler, IShellContainer shellContainer)
        {
            InitializeComponent();
            BindingContext = viewModel;

            // Upon constructing the page we know we can safely make our calls, hence we do it here. Otherwise, it would've been done elsewhere.
            this.Loaded += async (s, e) =>
            {
                if (_forcedNavigationAlready) return;

                // If we still have a valid username, we retrieve the lights and immediately load the correct lights + navigate to the LightsPage.
                var (isValid, username) = await hueHandler.IsOldConnectionValid();
                if (isValid)
                {
                    logger.LogInformation("Old Connection was valid with username {0}.", username);
                    viewModel.BoxText = username!;
                    viewModel.RefreshLightsCommand.Execute(null);
                    logger.LogInformation("Navigating to the Lights page.");
                    await shellContainer.CurrentShell.GoToAsync("///LightsPage");
                }
                else
                {
                    logger.LogInformation("Old Connection was invalid.");
                }

                _forcedNavigationAlready = true;
            };
        }
    }
}