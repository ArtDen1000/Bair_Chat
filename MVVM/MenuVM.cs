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

            //if(WebSocketManager.GetWebSocket() != null)  WebSocketManager.Disconnect();
			//Shell.Current.Navigated += (s, e) =>
			//{
			//	if(Shell. == this)
			//};

			Login = Preferences.Get("login", "User");
		}

		private bool CheckLogin() => !string.IsNullOrEmpty(Login);

        public async Task Join()
        {
            if (!string.IsNullOrEmpty(Id_room))
            {
				if (CheckLogin())
				{
					await WebSocketManager.Connect();
					ClientPrepareMessage message = new ClientPrepareMessage
					{
						action = ClientPrepareMessage.Action.Join,
						name = Login,
						room_code = int.Parse(Id_room.Replace(" ", string.Empty)),
					};
					await WebSocketManager.Send(message);

					await JoinRoom();
				}
			}
		}

		[RelayCommand]
		public async Task Create()
		{
            if (CheckLogin())
            {
                await WebSocketManager.Connect();

                ClientPrepareMessage message = new ClientPrepareMessage
                {
                    action = ClientPrepareMessage.Action.Create,
                    name = Login,
                };
                await WebSocketManager.Send(message);

                await JoinRoom();
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
