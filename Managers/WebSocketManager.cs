using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text.Json;

namespace RTCChat.Managers
{
    public static class WebSocketManager
    {
        private static ClientWebSocket webSocket;
		public enum Action
		{
			Join, Create
		}
		public static async Task<bool> Connect(string path, System.Action action)
        {
			try
			{
				webSocket = new ClientWebSocket();

				await webSocket.ConnectAsync(new Uri(Preferences.Get("ip", "ws://0.0.0.0:22") + path), CancellationToken.None);

				action.Invoke();

				Thread thread = new Thread(CheckState);
				thread.Start();

				return true;
			}
			catch
			{
				await Shell.Current.DisplayAlert("Ошибка", "Ошибка подключения", "Ок");
				return false;
			}

		}
        public static async Task<bool> Disconnect(System.Action action)
		{
			try
			{
				await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnect", CancellationToken.None);

				action.Invoke();

				return true;
			}
			catch
			{
				await Shell.Current.DisplayAlert("Ошибка", "Ошибка отключения", "Ок");
				return false;
			}
		}

		public static async Task Send<T>(T obj)
        {
			byte[] bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
			await webSocket.SendAsync(bytes, WebSocketMessageType.Text, true, default);
		}
		public static async Task Send(string text)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			await webSocket.SendAsync(bytes, WebSocketMessageType.Text, true, default);
		}
		public static async Task<string> Get()
        {
            byte[] buffer = new byte[4096];

			using var ms = new MemoryStream();
			WebSocketReceiveResult result;

			do
			{
				result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), default);
				ms.Write(buffer, 0, result.Count);
			}
			while (!result.EndOfMessage);

			ms.Seek(0, SeekOrigin.Begin);

			//var res = await webSocket.ReceiveAsync(buffer, default);
			return Encoding.UTF8.GetString(ms.ToArray());
		}

		public static ClientWebSocket GetWebSocket()
        {
            return webSocket;
		}



		public delegate void OnDisconnect(WebSocketState state);
		public static event OnDisconnect? onDisconnect;

		private static void CheckState(object obj)
		{
			try
			{
				while (true)
				{
					if (webSocket.State != WebSocketState.Open)
					{
						switch (webSocket.State)
						{
							case WebSocketState.Aborted:
								onDisconnect?.Invoke(webSocket.State);
								break;
						}
						return;
					}
				}
			}
			catch
			{
				Console.WriteLine("Error");
			}
			
		}
	}
}
