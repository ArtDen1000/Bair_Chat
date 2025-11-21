namespace RTCChat;
using MVVM;

public partial class Chat : ContentPage
{
	public Chat()
	{
		InitializeComponent();
		BindingContext = new ChatVM(this);
	}
}