using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCChat.DataTemplate
{
    public class MessageData
    {
        public string? text { get; set; }
        public string? time { get; set; }
        public string? client { get; set; }
        public bool isClient { get; set; }
        public int action { get; set; }
	}
}
