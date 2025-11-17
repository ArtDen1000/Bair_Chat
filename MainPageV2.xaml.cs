namespace RTCChat;
using MVVM;

public partial class MainPageV2 : ContentPage
{
	public MainPageV2()
	{
		InitializeComponent();
		BindingContext = new MenuVM(this);
	}
}