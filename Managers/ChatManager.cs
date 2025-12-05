using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTCChat.DataTemplate.Messages;
using Type = RTCChat.DataTemplate.Messages.MessageData.Type;

namespace RTCChat.Managers
{
	public class ChatManager
	{
		private VerticalStackLayout ChatLayout;
		public ChatManager(VerticalStackLayout chatLayout)
		{
			ChatLayout = chatLayout;
		}

		public void AddMessage(MessageData data)
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				switch (data.action)
				{
					case MessageData.Action.Sended:
						Grid inBorder = null;
						switch (data.type)
						{
							case Type.Text:
								{
									Label login = new Label()
									{
										Text = data.client,
										Style = Application.Current.Resources["Mini"] as Style,
										HorizontalTextAlignment = TextAlignment.Start
									};
									Label text = new Label()
									{
										Text = (data as TextMessage).formatText,
										FontSize = 18,
										Margin = new Thickness(10, 0),
									};
									Label time = new Label()
									{
										Text = data.time,
										Style = Application.Current.Resources["Mini"] as Style,
										HorizontalTextAlignment = TextAlignment.End
									};

									inBorder = new Grid()
									{
										RowDefinitions =
										{
											new RowDefinition(new GridLength(20, GridUnitType.Absolute)),
											new RowDefinition(new GridLength(1, GridUnitType.Auto)),
											new RowDefinition(new GridLength(20, GridUnitType.Absolute)),
										},
										ColumnDefinitions =
										{
											new ColumnDefinition(new GridLength(1, GridUnitType.Auto)),
										},
										Children =
										{
											login,
											text,
											time
										}
									};
									inBorder.SetRow(login, 0);
									inBorder.SetRow(text, 1);
									inBorder.SetRow(time, 2);
								}

								break;
							case Type.Picture:
								{
									Label login = new Label()
									{
										Text = data.client,
										Style = Application.Current.Resources["Mini"] as Style,
										HorizontalTextAlignment = TextAlignment.Start
									};

									Image image = new Image()
									{
										Source = (data as PictureMessage).uri,
										MaximumWidthRequest = Shell.Current.Window.Width * 0.8f
									};

									Label time = new Label()
									{
										Text = data.time,
										Style = Application.Current.Resources["Mini"] as Style,
										HorizontalTextAlignment = TextAlignment.End
									};

									inBorder = new Grid()
									{
										RowDefinitions =
										{
											new RowDefinition(new GridLength(20, GridUnitType.Absolute)),
											new RowDefinition(new GridLength(1, GridUnitType.Auto)),
											new RowDefinition(new GridLength(20, GridUnitType.Absolute)),
										},
										ColumnDefinitions =
										{
											new ColumnDefinition(new GridLength(1, GridUnitType.Auto)),
										},
										Children =
										{
											login,
											image,
											time
										}
									};
									inBorder.SetRow(login, 0);
									inBorder.SetRow(image, 1);
									inBorder.SetRow(time, 2);
								}
								break;
						}
						ChatLayout.Children.Add(new Border()
						{
							Style = Application.Current.Resources["MessageBorder1"] as Style,
							Content = inBorder,
							HorizontalOptions = data.isClient ? LayoutOptions.End : LayoutOptions.Start,
						});
						break;
					case MessageData.Action.Joined:
						ChatLayout.Children.Add(new Label()
						{
							Text = $"{data.client} присоединился",
							HorizontalTextAlignment = TextAlignment.Center,
						});
						break;
					case MessageData.Action.Leaved:
						ChatLayout.Children.Add(new Label()
						{
							Text = $"{data.client} отключился",
							HorizontalTextAlignment = TextAlignment.Center,
						});
						break;
				}
			});
		}
	}
}
