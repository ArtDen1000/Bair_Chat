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

		private const int k = 12;

		public string formatText
		{
			get
			{
				return text;
			}
			private set
			{
				int windowWidth = (int)Math.Round(Shell.Current.Window.Width * 0.8f / k);

				StringBuilder result = new StringBuilder();

				string[] line = value.Split('\n');
				for (int row = 0; row < line.Length; row++)
				{
					string[] words = line[row].Split(' ');

					for (int i = 0, size = 0, wordCount = 0; i < words.Length; i++)
					{
						if(words[i].Length > windowWidth)
						{
							result.Append(words[i].Substring(0, windowWidth - size));
							result.AppendLine();

							words[i] = words[i].Substring(windowWidth - size, words[i].Length - windowWidth + size);

							i--;
							size = 0;
						}
						else if(size + words[i].Length > windowWidth)
						{
							result.AppendLine();
							i--;
							size = 0;
						}
						else
						{
							result.Append(words[i] + ' ');
							size += words[i].Length;
						}
					}
					if (row != line.Length - 1) result.AppendLine();
				}



				text = result.ToString();
			}
		}
	}
}
