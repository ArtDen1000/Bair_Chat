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
using System.Linq;

namespace RTCChat.MVVM
{
	public partial class ChatVM : Overlay
	{
		private ContentPage parent;

		[ObservableProperty] private string login = "User";
		[ObservableProperty] private int id_room;
		[ObservableProperty] private int id_client;
		[ObservableProperty] private string message;
		[ObservableProperty] private string clients = "123123";
		[ObservableProperty] ObservableCollection<MessageData> messages = new ObservableCollection<MessageData>();

		private List<FileResult> selectedFiles;

		private Dictionary<string, string> _clients;

		public ChatVM(ContentPage page)
		{
			parent = page;

			Id_room = (int)DataTransportManager.GetData("id_room");
			Id_client = (int)DataTransportManager.GetData("id_client");
			Login = (string)DataTransportManager.GetData("login");

			_clients = (Dictionary<string, string>)DataTransportManager.GetData("client_names");
			Clients =  string.Join('\n', _clients.Values);

			DataTransportManager.ClearData();

			Task.Run(ReceiveMessage);
		}

		public async Task ReceiveMessage()
		{
			while (true)
			{
				var res = await WebSocketManager.Get();
				ServerRoomMessage roomMessage = JsonSerializer.Deserialize<ServerRoomMessage>(res);
				//Messages.Add(new MessageData()
				//{
				//	text = res, action = 0
				//});
				switch (roomMessage.action)
				{
					case ServerRoomMessage.Action.Joined:
						Messages.Add(new MessageData()
						{
							text = $"{roomMessage.message_data} присоединился!",
							action = 1
						});
						_clients.Add(roomMessage.client_id.ToString(), roomMessage.message_data);
						Clients = string.Join('\n', _clients.Values);
						break;
					case ServerRoomMessage.Action.Leaved:
						Messages.Add(new MessageData()
						{
							text = $"{_clients[roomMessage.client_id.ToString()]} отключился!",
							action = 1
						});
						_clients.Remove(roomMessage.client_id.ToString());
						Clients = string.Join('\n', _clients.Values);
						break;
					case ServerRoomMessage.Action.Sended:
						Messages.Add(new MessageData()
						{
							text = roomMessage.message_data,
							time = DateTime.Now.ToString("HH:mm"),
							client = _clients[roomMessage.client_id.ToString()],
							isClient = roomMessage.client_id == Id_client,
							action = 0
						});
						break;
				}
				
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
			Message = Message.Trim();
			if (!string.IsNullOrEmpty(Message))
			{
				await WebSocketManager.Send(Message);
				Message = string.Empty;
			}
		}
		[RelayCommand]
		public void Disconnect()
		{
			WebSocketManager.Disconnect();
		}

		[RelayCommand]
		public void CopyIdRoom()
		{
			Clipboard.SetTextAsync(Id_room.ToString());
		}
	}
}
