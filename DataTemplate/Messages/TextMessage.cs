using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCChat.DataTemplate.Messages
{
	public class TextMessage : MessageData
	{
		public TextMessage(string text, string client) : base(client, Action.Sended)
		{
			type = Type.Text;
			formatText = text;
		}

		private string text;

		public string formatText
		{
			get
			{
				return text;
			}
			private set
			{
				StringBuilder result = new StringBuilder();

				string[] line = value.Split('\n');
				for (int row = 0; row < line.Length; row++)
				{
					int[] wordSize;
					string[] words = line[row].Split(' ');

					wordSize = (from p in words select p.Length * 12).ToArray();

					int tmp = 0;
					for (int i = 0; i < wordSize.Length; i++)
					{
						tmp += wordSize[i];
						if (tmp > (int)Math.Round(Shell.Current.Window.Width * 0.8f))
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
					if (row != line.Length - 1) result.AppendLine();
				}



				text = result.ToString();
			}
		}
	}
}
