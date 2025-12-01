using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCChat.DataTemplate
{
    public struct ClientPrepareMessage
    {
		public enum Action
		{
			Join,
			Create,
		}

		public Action action { get; set; }
		public string name { get; set; }
		public int? room_code { get; set; }
	}
}
