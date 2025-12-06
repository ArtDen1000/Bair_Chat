namespace RTCChat;

using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Mvvm.Input;
using MVVM;

public partial class MainPageV2 : ContentPage
{
	private MenuVM viewModel;
	public MainPageV2()
	{
		InitializeComponent();
		viewModel = new MenuVM();
		BindingContext = viewModel;
		viewModel.OverlayManager.SetOrigin(OverlayBorder);
	}

	protected override bool OnBackButtonPressed()
	{
		viewModel.OverlayManager.CloseOverlay();

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

	private void ChangeLogin(object sender, EventArgs e) => viewModel.ChangeLogin();

	private void Join(object sender, EventArgs e)
	{
		Label info = new Label
		{
			Text = "Введите ID комнаты",
			FontAttributes = FontAttributes.Bold,
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center
		};

		Entry roomId = new Entry
		{
			Placeholder = "0 0 0 0",
			Keyboard = Keyboard.Text,
			TextTransform = TextTransform.Uppercase,
			Behaviors =
			{
				new MaskedBehavior
				{
					Mask = "X X X X"
				}
			},
			HorizontalTextAlignment = TextAlignment.Center,
		};
		roomId.SetBinding(Entry.TextProperty, "Id_room_format");

		Button join = new Button
		{
			Style = Application.Current.Resources["ContrastButton"] as Style,
			Text = $"Подключиться",
			Command = new Command(async () =>
			{
				await viewModel.Join();
			}),
		};

		Grid grid = new Grid
		{
			RowDefinitions = {
					new RowDefinition(new GridLength(1, GridUnitType.Auto)),
					new RowDefinition(new GridLength(1, GridUnitType.Auto)),
					new RowDefinition(new GridLength(1, GridUnitType.Auto)),
				},
			RowSpacing = 20,
			Children =
				{
					info,
					roomId,
					join,
				}
		};
		grid.SetRow(info, 0);
		grid.SetRow(roomId, 1);
		grid.SetRow(join, 2);

		viewModel.OverlayManager.AddOverlay(grid);
		viewModel.OverlayManager.ShowOverlay();
	}
}