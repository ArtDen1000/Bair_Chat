using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RTCChat.DataTemplate;
using RTCChat.Managers;

namespace RTCChat.MVVM
{
	public partial class ChatVM : ObservableObject
	{
		private ContentPage parent;

		private int id_room;
		private int id_client;

		public ChatVM(ContentPage page)
		{
			parent = page;

			id_room = (int)DataTransportManager.GetData("id_room");
			id_client = (int)DataTransportManager.GetData("id_client");
			Login = (string)DataTransportManager.GetData("login");

			DataTransportManager.ClearData();

			Task.Run(ReceiveMessage);
		}

		[ObservableProperty] private string login = "User";
		[ObservableProperty] private string message;
		[ObservableProperty] ObservableCollection<MessageData> messages = new ObservableCollection<MessageData>();

		private List<FileResult> selectedFiles;

		public async Task ReceiveMessage()
		{
			while (true)
			{
				var res = await WebSocketManager.Get();
				ServerRoomMessage message = JsonSerializer.Deserialize<ServerRoomMessage>(res);
				Messages.Add(new MessageData()
				{
					text = message.message,
				});
				//messages.Add(new MessageData()
				//{
				//	text = res,
				//});
			}
		}

		[RelayCommand]
		public void Call()
		{

		}
		[RelayCommand]
		public async Task<string[]> File()
		{
			try
			{
				var files = await FilePicker.Default.PickMultipleAsync();
				selectedFiles = files.ToList();
				await parent.DisplayAlert($"{selectedFiles.Count} files Selected", string.Join('\n', from f in selectedFiles select f.FileName), "Ok");
				return (from f in selectedFiles select f.FileName).ToArray();

			}
			catch (Exception ex)
			{
				await parent.DisplayAlert("Error", ex.Message, "Ok");
			}

			return null;
		}
		[RelayCommand]
		public async Task SendMessage()
		{
			if (!string.IsNullOrEmpty(Message))
			{
				await WebSocketManager.Send(Message);
				Message = string.Empty;
			}
				
		}
	}
}
