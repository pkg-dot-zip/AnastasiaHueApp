using AnastasiaHueApp.ViewModels;

namespace AnastasiaHueApp
{
    public partial class LightsPage : ContentPage
    {
        public LightsPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}