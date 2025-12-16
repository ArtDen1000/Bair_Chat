using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Platform;
using RTCChat.DataTemplate;
using RTCChat.Managers;

namespace RTCChat.MVVM
{
    public partial class MenuVM : ObservableObject
    {
		[ObservableProperty] private string title = string.Empty;
        [ObservableProperty] public string login = "User";
        [ObservableProperty] public string id_room_format = string.Empty;
		[ObservableProperty] public bool isOverlay;


		private string id_room { get => Id_room_format.Length > 0 ? Id_room_format.Replace(" ", string.Empty) : string.Empty; set => Id_room_format = value; }

		private string[] titles =
        {
            "До бурята не дошли",
            "Приватный чат",
            "Also try Max!",
            "Сделано на коленке",
            "♥ ♥ ♥",
            "Ловит даже в кладовке!"
        };

		public Overlay OverlayManager = new Overlay();

		private Random random = new Random();
        public MenuVM()
        {
			Title = titles[random.Next(0, titles.Length)];

			Login = Preferences.Get("login", "User");

			if(!Preferences.Get("isSetTheme", false))
			{
				OpenColorSettings();
				Preferences.Set("isSetTheme", true);
			}

			OverlayManager.OverlayVisibleChanged += (isVisible) => IsOverlay = isVisible;
		}

		//Connect

		private bool CheckLogin() => !string.IsNullOrEmpty(Login);
		private bool CheckId() => !string.IsNullOrEmpty(id_room);

		public async Task Join()
        {
            if (CheckLogin() && CheckId())
            {
				await WebSocketManager.Connect($"/join?name={Login}&room={id_room}", async () => await JoinRoom());
			}
		}

		private async Task JoinRoom()
		{
			string res = await WebSocketManager.Get();


			ServerPrepareResponse response = JsonSerializer.Deserialize<ServerPrepareResponse>(res);
			if (response.error != null) return;

			DataTransportManager.SetData(("id_room", response.room_code),
				("id_client", response.client_id),
				("login", Login),
				("client_names", response.client_names));

			await Shell.Current.Navigation.PushAsync(new Chat());
		}

		[RelayCommand]
		public async Task Create()
		{
            if (CheckLogin())
            {
                await WebSocketManager.Connect($"/create?name={Login}", async () => await JoinRoom());
			}
		}

		//Other

		[RelayCommand]
		public void ChangeLogin()
		{
			Login = Login.Trim();
			Preferences.Set("login", Login);
		}

		[RelayCommand]
		public async Task OpenColorSettings()
		{
			await Shell.Current.Navigation.PushAsync(new ColorSettings());
		}

		[RelayCommand]
		public void CloseOverlay() => OverlayManager.CloseOverlay();
	}
}
