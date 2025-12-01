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
            await webSocket.ConnectAsync(new Uri("ws://95.31.137.235:8888"), default);
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
            byte[] buffer = new byte[1024];
            var res = await webSocket.ReceiveAsync(buffer, default);
            return Encoding.UTF8.GetString(buffer, 0, res.Count);
		}

		public static ClientWebSocket GetWebSocket()
        {
            return webSocket;
		}
	}
}
