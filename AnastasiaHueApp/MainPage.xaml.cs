using AnastasiaHueApp.Util.Hue;
using AnastasiaHueApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel, ILogger<LightsPage> logger, IHueHandler hueHandler)
        {
            InitializeComponent();
            BindingContext = viewModel;

            // Upon constructing the page we know we can safely make our calls, hence we do it here. Otherwise, it would've been done elsewhere.
            this.Loaded += async (s, e) =>
            {
                if (await hueHandler.IsOldConnectionValid())
                {
                    logger.LogInformation("Old Connection was valid.");
                    viewModel.RefreshLightsCommand.Execute(null);
                    logger.LogInformation("Navigating to the Lights page.");
                    await Shell.Current.GoToAsync("///LightsPage");
                }
                else
                {
                    logger.LogInformation("Old Connection was invalid.");
                }
            };
        }
    }
}