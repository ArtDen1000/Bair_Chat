using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCChat.DataTemplate
{
    public class MessageData
    {
        public string text { get; set; }
        public string? time { get; set; }
        public string? client { get; set; }
		public int widthArea { get; set; }
		public bool isClient { get; set; }
        public int action { get; set; }

        public string _text
        {
            get
            {
				StringBuilder result = new StringBuilder();

				string[] line = text.Split('\n');
				for (int row = 0; row < line.Length; row++)
                {
					int[] wordSize;
					string[] words = line[row].Split(' ');

					wordSize = (from p in words select p.Length * 12).ToArray();

					int tmp = 0;
					for (int i = 0; i < wordSize.Length; i++)
					{
						tmp += wordSize[i];
						if (tmp > (int)Math.Round(widthArea * 0.9f))
						{
							i--;
							result.AppendLine();
							tmp = 0;
						}
						else
						{
							result.Append(words[i] + ' ');
						}
					}
					if(row != line.Length - 1) result.AppendLine();
				}

                
                
                return result.ToString();
			}
		}
	}
}
