using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCChat.DataTemplate.Messages
{
	public class SquareMessage : MessageData
	{
		public SquareMessage(Uri uri, string client) : base(client, Action.Sended)
		{
			type = Type.Square;
		}
	}
}
