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
				Stream s = await FileSystem.OpenAppPackageFileAsync("ip.txt");
				StreamReader reader = new StreamReader(s);

				Preferences.Set("ip", reader.ReadToEnd());
			});
            return new Window(new AppShell());
        }
    }
}