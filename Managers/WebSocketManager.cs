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
		public static async Task Connect()
        {
			webSocket = new ClientWebSocket();

			await webSocket.ConnectAsync(new Uri(Preferences.Get("ip", "ws://0.0.0.0:22")), default);

			Thread thread = new Thread(CheckState);
			thread.Start();
		}
        public static void Disconnect()
		{
			Shell.Current.Navigation.PopToRootAsync();
			webSocket.Dispose();
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



		public delegate void OnDisconnect();
		public static event OnDisconnect? onDisconnect;

		private static async void CheckState(object obj)
		{
			while (true)
			{
				if(webSocket.State != WebSocketState.Open)
				{
					onDisconnect?.Invoke();
					//await Shell.Current.GoToAsync("//MainPage");
					await Shell.Current.Navigation.PopToRootAsync();

					return;
				}
			}
		}
	}
}
