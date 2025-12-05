using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RTCChat.Managers;
using RTCChat.DataTemplate;
using System.Text.Json;

namespace RTCChat.MVVM
{
    public partial class MenuVM : Overlay
    {
		[ObservableProperty] private string title = string.Empty;
        [ObservableProperty] public string login = "User";
        [ObservableProperty] public string id_room;

		private string[] titles =
        {
            "До бурята не дошли",
            "Приватный чат",
            "Also try Max!",
            "Сделано на коленке",
            "♥ ♥ ♥",
            "Ловит даже в кладовке!"
        };

        private Random random = new Random();
        public MenuVM()
        {
			Title = titles[random.Next(0, titles.Length)];

			Login = Preferences.Get("login", "User");
		}

		private bool CheckLogin() => !string.IsNullOrEmpty(Login);
		private bool CheckId() => !string.IsNullOrEmpty(Id_room);

		public async Task Join()
        {
            if (CheckLogin() && CheckId())
            {
				await WebSocketManager.Connect($"/join?name={Login}&room={int.Parse(Id_room.Replace(" ", string.Empty))}", async () => await JoinRoom());
			}
		}

		[RelayCommand]
		public async Task Create()
		{
            if (CheckLogin())
            {
                await WebSocketManager.Connect($"/create?name={Login}", async () => await JoinRoom());
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


			//await Shell.Current.GoToAsync("//ChatPage");
			await Shell.Current.Navigation.PushAsync(new Chat());
		}


		[RelayCommand]
		public void ChangeLogin()
		{
			Login = Login.Trim();
			Preferences.Set("login", Login);
		}

	}
}
