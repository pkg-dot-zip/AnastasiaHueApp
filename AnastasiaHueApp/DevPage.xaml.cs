using AnastasiaHueApp.Util.Hue;
using AnastasiaHueApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp
{
    public partial class DevPage : ContentPage
    {
        public DevPage(DevViewModel viewModel, ILogger<LightsPage> logger, IHueHandler hueHandler)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}