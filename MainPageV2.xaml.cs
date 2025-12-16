namespace RTCChat;

using System;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
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
		List<string> codes = (from code in Preferences.Get("favoritsCodes", string.Empty).Split(' ') where !string.IsNullOrEmpty(code) select code).ToList();


		Label info = new Label
		{
			Text = "Введите ID комнаты",
			FontAttributes = FontAttributes.Bold,
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center
		};


		Entry roomEntry = new Entry
		{
			Placeholder = "X X X X",
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
		roomEntry.SetBinding(Entry.TextProperty, "Id_room_format");

		Image addFavoriteButton = new Image()
		{
			Source = "heart_clear.png",
		};
		addFavoriteButton.Behaviors.Add(new IconTintColorBehavior()
		{
			TintColor = Application.Current.Resources["Red"] as Color
		});
		addFavoriteButton.GestureRecognizers.Add(new TapGestureRecognizer()
		{
			Command = new Command(() =>
			{
				codes.Add(roomEntry.Text.Replace(" ", ""));
				Preferences.Set("favoritsCodes", codes.Count == 0 ? string.Empty : string.Join(' ', codes));
				viewModel.OverlayManager.CloseOverlay();

				Join(sender, e);
			})
		});

		Grid roomId = new Grid()
		{
			HeightRequest = 54,
			ColumnDefinitions =
				{
					new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
					new ColumnDefinition(new GridLength(40, GridUnitType.Absolute)),
				},
			Children =
				{
					roomEntry, addFavoriteButton
				}
		};
		roomId.SetColumn(roomEntry, 0);
		roomId.SetColumn(addFavoriteButton, 1);

		VerticalStackLayout favorits = new VerticalStackLayout();

		ScrollView scrollView = new ScrollView()
		{
			MaximumHeightRequest = 200,
			Content = favorits
		};

		for (int i = 0; i < codes.Count; i++)
		{
			int index = i;

			Label name = new Label()
			{
				Text = codes[i],
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
			};
			name.GestureRecognizers.Add(new TapGestureRecognizer()
			{
				Command = new Command(() =>
				{
					roomEntry.Text = codes[index];
				})
			});

			Image delFavoriteButton = new Image()
			{
				Source = "heart.png",
			};
			delFavoriteButton.Behaviors.Add(new IconTintColorBehavior()
			{
				TintColor = Application.Current.Resources["Red"] as Color
			});
			delFavoriteButton.GestureRecognizers.Add(new TapGestureRecognizer()
			{
				Command = new Command(() =>
				{
					codes.RemoveAt(index);
					Preferences.Set("favoritsCodes", codes.Count == 0 ? string.Empty : string.Join(' ', codes));
					viewModel.OverlayManager.CloseOverlay();

					Join(sender, e);
				})
			});
			Grid item = new Grid()
			{
				HeightRequest = 40,
				ColumnDefinitions =
				{
					new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
					new ColumnDefinition(new GridLength(40, GridUnitType.Absolute)),
				},
				Children =
				{
					name, delFavoriteButton,
				}
			};

			item.SetColumn(name, 0);
			item.SetColumn(delFavoriteButton, 1);

			favorits.Children.Add(item);
		}
		

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
					new RowDefinition(new GridLength(1, GridUnitType.Auto)),
				},
			RowSpacing = 20,
			Children =
				{
					info,
					roomId,
					scrollView,
					join,
				}
		};
		grid.SetRow(info, 0);
		grid.SetRow(roomId, 1);
		grid.SetRow(scrollView, 2);
		grid.SetRow(join, 3);

		viewModel.OverlayManager.AddOverlay(grid);
		viewModel.OverlayManager.ShowOverlay();
	}
}