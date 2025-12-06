using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RTCChat.Managers;

namespace RTCChat.MVVM
{
    public partial class ColorSettingsVM : ObservableObject
	{
		public enum ThemeType
		{
			Light,
			System,
			Dark
		}
		[ObservableProperty] public string theme = "1";
		[ObservableProperty] public string ip = Preferences.Get("ip", "0.0.0.0");

		public ColorSettingsVM()
		{
			theme = ((int)App.Current.UserAppTheme).ToString();
		}

		[RelayCommand]
        public void ChangeTheme(string theme)
        {
			Theme = theme;
			App.Current.UserAppTheme = theme switch { "1" => AppTheme.Light, "2" => AppTheme.Dark, _ => AppTheme.Unspecified };
			Preferences.Set("theme", (int)App.Current.UserAppTheme);
        }
		[RelayCommand]
		public void SetColor(string color)
		{
			App.Current.Resources["ContrastCustom"] = App.Current.Resources[color];
			Preferences.Set("customContrastColor", color);
		}
		[RelayCommand]
		public async Task Apply()
		{
			await Shell.Current.Navigation.PopToRootAsync();
		}

		public void ChangeIP()
		{
			Preferences.Set("ip", Ip);
		}
	}
}
