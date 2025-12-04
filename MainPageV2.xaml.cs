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
		viewModel.SetOrigin(OverlayBorder);
	}

	private void ChangeLogin(object sender, FocusEventArgs e) => viewModel.ChangeLogin();

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
			Keyboard = Keyboard.Numeric,
			Behaviors =
			{
				new MaskedBehavior
				{
					Mask = "X X X X"
				}
			},
			HorizontalTextAlignment = TextAlignment.Center,
		};
		roomId.SetBinding(Entry.TextProperty, "Id_room");

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

		viewModel.AddOverlay(grid);
		viewModel.ShowOverlay();
	}
}