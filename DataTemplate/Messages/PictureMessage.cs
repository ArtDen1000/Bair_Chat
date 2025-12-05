using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RTCChat.DataTemplate.Messages
{
	public class PictureMessage : MessageData
	{
		public string uri { get; private set; }

		public PictureMessage(string uri, string client) : base(client, Action.Sended)
		{
			this.uri = uri;
			type = Type.Picture;
		}
	}
}
