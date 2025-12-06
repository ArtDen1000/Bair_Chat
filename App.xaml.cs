using Microsoft.Maui.Platform;


namespace RTCChat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Task.Run(async () =>
            {
#if RELEASE
                Stream s = await FileSystem.OpenAppPackageFileAsync("ip.txt");
#else
                Stream s = await FileSystem.OpenAppPackageFileAsync("ip_debug.txt");
#endif
                StreamReader reader = new StreamReader(s);

				Preferences.Set("ip", reader.ReadToEnd());
			});
            App.Current.UserAppTheme = (AppTheme)(Preferences.Get("theme", 0));
            App.Current.Resources["ContrastCustom"] = App.Current.Resources[Preferences.Get("customContrastColor", "Contrast1")];
            return new Window(new AppShell());
        }
	}
}