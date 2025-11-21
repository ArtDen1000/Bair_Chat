using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;

namespace RTCChat.MVVM
{
	public partial class ChatVM : ObservableObject
	{
		private ContentPage parent;

		public ChatVM(ContentPage page)
		{
			parent = page;
		}

		[ObservableProperty] private string login = "User";
		[ObservableProperty] private string message;

		private List<FileResult> selectedFiles;

		[RelayCommand]
		public void Call()
		{

		}
		[RelayCommand]
		public async Task<string[]> File()
		{
			try
			{
				var files = await FilePicker.Default.PickMultipleAsync();
				selectedFiles = files.ToList();
				await parent.DisplayAlert($"{selectedFiles.Count} files Selected", string.Join('\n', from f in selectedFiles select f.FileName), "Ok");
				return (from f in selectedFiles select f.FileName).ToArray();

			}
			catch (Exception ex)
			{
				await parent.DisplayAlert("Error", ex.Message, "Ok");
			}

			return null;
		}
		[RelayCommand]
		public async Task SendMessage()
		{
			if (!string.IsNullOrEmpty(Message))
			{
				await parent.DisplayAlert("Message", $"Send: {Message}", "Ok");
			}
				
		}
	}
}
