using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCChat.DataTemplate
{
    public struct ServerPrepareResponse
    {
        public enum ServerPrepareError
        {
            InvalidJson,
			InvalidRoomCode
		}


		public int? client_id { get; set; }
		public int? room_code { get; set; }
		public ServerPrepareError? error { get; set; }

	}
}
