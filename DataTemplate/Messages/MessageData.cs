using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCChat.DataTemplate.Messages
{
    public class MessageData
    {
        public enum Type
        {
            Text, Picture, Video, File, Square
		}
		public enum Action
		{
			Sended,
			Joined,
			Leaved
		}
		public Type type { get; protected set; }
		public string? time { get; private set; }
        public string client { get; set; }
		public bool isClient { get; set; }
        public Action action { get; set; }

        public MessageData(string client, Action action)
        {
            this.client = client;
			this.action = action;
			time = DateTime.Now.ToString("HH:mm");
		}
	}
}
