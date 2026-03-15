using Microsoft.Extensions.DependencyInjection;

namespace StoneLedger
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = new Window(new AppShell());

            // Example: set window to 1/3 of the screen
            var displayInfo = DeviceDisplay.MainDisplayInfo;

            double screenWidth = displayInfo.Width / displayInfo.Density;
            double screenHeight = displayInfo.Height / displayInfo.Density;

            window.Width = screenWidth / 3;
            window.Height = screenHeight / 2;

            // Optional: center it
            window.X = (screenWidth - window.Width) / 2;
            window.Y = (screenHeight - window.Height) / 1.5;

            return window;
        }
    }
}