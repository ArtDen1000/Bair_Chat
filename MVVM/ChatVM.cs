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
using RTCChat.DataTemplate.Messages;
using System.Collections;
using System.Net.Mime;

namespace RTCChat.MVVM
{
	public partial class ChatVM : ObservableObject
	{
		[ObservableProperty] private string login = "User";
		[ObservableProperty] private string id_room;
		[ObservableProperty] private int id_client;
		[ObservableProperty] private string message;
		[ObservableProperty] private string clients;
		[ObservableProperty] private bool isOverlay;

		private List<FileResult> selectedFiles;

		private Dictionary<string, string> _clients;

		public ChatManager ChatManager;
		public Overlay OverlayManager = new Overlay();

		public ChatVM()
		{
			Id_room = (string?)DataTransportManager.GetData("id_room");
			Id_client = (int)DataTransportManager.GetData("id_client");
			Login = (string)DataTransportManager.GetData("login");

			_clients = (Dictionary<string, string>)DataTransportManager.GetData("client_names");
			Clients =  string.Join('\n', _clients.Values);

			DataTransportManager.ClearData();

			Task.Run(ReceiveMessage);

			OverlayManager.OverlayVisibleChanged += (isVisible) => IsOverlay = isVisible;
		}

		public async Task ReceiveMessage()
		{
			while (true)
			{
				var res = await WebSocketManager.Get();
				ServerRoomMessage roomMessage = JsonSerializer.Deserialize<ServerRoomMessage>(res);

				switch (roomMessage.action)
				{
					case ServerRoomMessage.Action.Joined:
						_clients.Add(roomMessage.client_id.ToString(), roomMessage.message_data);
						Clients = string.Join('\n', _clients.Values);

						ChatManager.AddMessage(new MessageData(_clients[roomMessage.client_id.ToString()], MessageData.Action.Joined));
						break;
					case ServerRoomMessage.Action.Leaved:
						ChatManager.AddMessage(new MessageData(_clients[roomMessage.client_id.ToString()], MessageData.Action.Leaved));

						_clients.Remove(roomMessage.client_id.ToString());
						Clients = string.Join('\n', _clients.Values);
						break;
					case ServerRoomMessage.Action.Sended:
						ChatManager.AddMessage(new TextMessage(roomMessage.message_data, _clients[roomMessage.client_id.ToString()])
						{
							isClient = roomMessage.client_id == Id_client,
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
				//await Shell.Current.DisplayAlert($"{selectedFiles.Count} files Selected", string.Join('\n', from f in selectedFiles select f.FileName), "Ok");
				foreach(string uri in from file in selectedFiles select file.FullPath)
				{
					ChatManager.AddMessage(new PictureMessage(uri, Login)
					{
						isClient = true,
					});
				}
				return (from f in selectedFiles select f.FileName).ToArray();

			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
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
		public void CloseOverlay() => OverlayManager.CloseOverlay();
	}
}
