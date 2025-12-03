namespace RTCChat;
using MVVM;
using RTCChat.Managers;

public partial class Chat : ContentPage
{
	public Chat()
	{
		InitializeComponent();
		ChatVM page = new ChatVM(this);
		BindingContext = page;

		ChatLayout.SizeChanged += async (s, e) =>
		{
			await Scroll.ScrollToAsync(ChatLayout, ScrollToPosition.End, false);
		};
		MessageEntry.Completed += async (s, e) =>
		{
			await page.SendMessage();
		};
	}
	protected override bool OnBackButtonPressed()
	{
		WebSocketManager.Disconnect();
		return true;

		// // Use the line above if you want to just disable the Back action. 
		// // If you want to instead bind it to the same command as 
		// // the BackButtonBehavior, use something like this :
		//
		// if (BindingContext is BaseViewModel vm)
		// {
		//     vm.BackButtonPressed();
		//     return true;
		// }
		// return false;
	}
}