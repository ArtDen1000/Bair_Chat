namespace RTCChat;

using CommunityToolkit.Mvvm.Input;
using MVVM;

public partial class MainPageV2 : ContentPage
{
	private MenuVM model;
	public MainPageV2()
	{
		InitializeComponent();
		model = new MenuVM(this);
		BindingContext = model;
	}

	private void ChangeLogin(object sender, FocusEventArgs e) => model.ChangeLogin();
}