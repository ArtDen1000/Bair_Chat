namespace RTCChat;

using MVVM;
using RTCChat.Managers;

public partial class Chat : ContentPage
{
	private ChatVM viewModel;
	public int widthArea => (int)(ChatLayout.Width);
	public Chat()
	{
		InitializeComponent();
		viewModel = new ChatVM();
		BindingContext = viewModel;

		viewModel.ChatManager = new ChatManager(ChatLayout);

		ChatLayout.SizeChanged += async (s, e) =>
		{
			if(Scroll.ScrollY + Scroll.Height > ChatLayout.Height - 200)
				await Scroll.ScrollToAsync(ChatLayout, ScrollToPosition.End, false);
		};
		MessageEntry.Completed += async (s, e) =>
		{
			await viewModel.SendMessage();
		};

		viewModel.OverlayManager.SetOrigin(OverlayBorder);
	}
	protected override bool OnBackButtonPressed()
	{
		if (viewModel.OverlayManager.CloseOverlay()) return true;

		ShowDisconnect();

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
	public void ShowDisconnect()
	{
		Label info = new Label
		{
			Text = "Отключиться?",
			FontAttributes = FontAttributes.Bold,
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center
		};

		Button disconnect = new Button
		{
			Style = Application.Current.Resources["ContrastButton2"] as Style,
			BorderColor = Application.Current.Resources["Red"] as Color,
			Text = $"Да",
			Margin = new Thickness(0, 20, 0, 0),
			Command = new Command(async() =>
			{
				await WebSocketManager.Disconnect(async () => await Shell.Current.Navigation.PopToRootAsync());
			}),
		};

		Grid grid = new Grid
		{
			RowDefinitions = {
					new RowDefinition(new GridLength(1, GridUnitType.Auto)),
					new RowDefinition(new GridLength(1, GridUnitType.Auto)),
				},
			Children =
				{
					info,
					disconnect,
				}
		};
		grid.SetRow(info, 0);
		grid.SetRow(disconnect, 1);

		viewModel.OverlayManager.AddOverlay(grid);
		viewModel.OverlayManager.ShowOverlay();
	}
	public void ShowInfo(object sender, EventArgs e)
	{
		Label info = new Label
		{
			Text = "Информация",
			FontAttributes = FontAttributes.Bold,
			HorizontalTextAlignment = TextAlignment.Start,
			VerticalTextAlignment = TextAlignment.Center
		};
		Label clients = new Label
		{
			Text = "Участники:",
			FontAttributes = FontAttributes.Bold,
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			Margin = new Thickness(0,20,0,0),
		};
		Label names = new Label
		{
			HorizontalTextAlignment = TextAlignment.Center
		};
		names.SetBinding(Label.TextProperty, "Clients");

		Button id = new Button
		{
			Text = $"ID: {viewModel.Id_room.ToString()}",
			HorizontalOptions = LayoutOptions.End,
			Padding = new Thickness(5, 0),
			Command = new Command(() =>
			{
				Clipboard.SetTextAsync(viewModel.Id_room.ToString());
			}),
		};
		Button disconnect = new Button
		{
			Style = Application.Current.Resources["ContrastButton2"] as Style,
			BorderColor = Application.Current.Resources["Red"] as Color,
			Text = $"Отключиться",
			Margin = new Thickness(0,20,0,0),
			Command = new Command(ShowDisconnect),
		};

		ScrollView scroll = new ScrollView
		{
			Content = names,
			MaximumHeightRequest = 200,
		};

		Grid grid = new Grid
		{
			RowDefinitions = {
					new RowDefinition(new GridLength(1, GridUnitType.Auto)),
					new RowDefinition(new GridLength(1, GridUnitType.Auto)),
					new RowDefinition(new GridLength(1, GridUnitType.Auto)),
					new RowDefinition(new GridLength(1, GridUnitType.Auto)),
				},
			Children =
				{
					info,
					id,
					clients,
					disconnect,
					scroll
				}
		};
		grid.SetRow(info, 0);
		grid.SetRow(clients, 1);
		grid.SetRow(id, 0);
		grid.SetRow(disconnect, 3);
		grid.SetRow(scroll, 2);

		viewModel.OverlayManager.AddOverlay(grid);
		viewModel.OverlayManager.ShowOverlay();
	}
}