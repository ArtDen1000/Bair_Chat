using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RTCChat.DataTemplate.Messages
{
	public class VideoMessage : MessageData
	{
		public VideoMessage(Uri uri, string client) : base(client, Action.Sended)
		{
			type = Type.Video;
		}
	}
}
