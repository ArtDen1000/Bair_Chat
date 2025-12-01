using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RTCChat.Managers;
using RTCChat.DataTemplate;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;

namespace RTCChat.MVVM
{
    public partial class MenuVM : ObservableObject
    {
        private ContentPage parent;

		[ObservableProperty] private string title = string.Empty;
        [ObservableProperty] private string login = "User";

        private string[] titles =
        {
            "До бурята не дошли",
            "Приватный чат",
            "Also try Max!",
            "Сделано на коленке",
            "♥ ♥ ♥"
        };

        private Random random = new Random();
        public MenuVM(ContentPage page)
        {
            parent = page;
			Title = titles[random.Next(0, titles.Length)];

            
		}

        private bool CheckLogin() => !string.IsNullOrEmpty(Login);

        [RelayCommand]
        public async Task Join()
        {
            if (CheckLogin())
            {
				
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

                string res = await WebSocketManager.Get();
                ServerPrepareResponse response = JsonSerializer.Deserialize<ServerPrepareResponse>(res);
                await parent.DisplayAlert("Info", $"Room code: {response.room_code}", "OK");
				DataTransportManager.SetData(("id_room", response.room_code.Value),
                    ("id_client", response.client_id.Value),
                    ("login", Login));


				await Shell.Current.GoToAsync("//ChatPage");
			}
		}
	}
}
