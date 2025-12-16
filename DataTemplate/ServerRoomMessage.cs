using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCChat.DataTemplate
{
    public struct ServerRoomMessage
    {
        public enum Action
        {
            Sended,
            Joined,
            Leaved,
            Rejoined
		}

        public Action action { get; set; }
		public int client_id { get; set; }
		public string? message_data { get; set; }

	}
}
