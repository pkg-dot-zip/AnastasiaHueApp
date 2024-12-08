namespace AnastasiaHueApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Force dark mode.
            Application.Current!.UserAppTheme = AppTheme.Dark;
            this.RequestedThemeChanged += (s, e) => { Application.Current.UserAppTheme = AppTheme.Dark; };
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}