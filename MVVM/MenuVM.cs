using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RTCChat.MVVM
{
    public partial class MenuVM : ObservableObject
    {
        private ContentPage parent;

		[ObservableProperty] private string title = string.Empty;
        [ObservableProperty] private string login = "User";

        private string[] titles =
        {
            "До бурята не дошли",
            "Приватный чат",
            "Also try Max!",
            "Сделано на коленке",
            "♥ ♥ ♥"
        };

        Random random = new Random();
        public MenuVM(ContentPage page)
        {
            parent = page;
			Title = titles[random.Next(0, titles.Length)];
		}

        private bool CheckLogin() => !string.IsNullOrEmpty(Login);

        [RelayCommand]
        public void Enter()
        {
            if (CheckLogin())
            {
                parent.DisplayAlert("Message", "Enter", "Ok");
            }
        }

		[RelayCommand]
		public void Create()
		{
			if (CheckLogin())
			{
                parent.DisplayAlert("Message", $"Create room {random.Next(1000, 9999)}", "Ok");
			}
		}
	}
}
