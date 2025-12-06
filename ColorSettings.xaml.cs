namespace RTCChat;

using RTCChat.MVVM;

public partial class ColorSettings : ContentPage
{
	ColorSettingsVM viewModel;
	public ColorSettings()
	{
		InitializeComponent();
		BindingContext = viewModel = new ColorSettingsVM();
	}

	private void ChangeIP(object sender, EventArgs e) => viewModel.ChangeIP();

}