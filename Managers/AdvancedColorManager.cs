using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if ANDROID
using Android.Views;
using Microsoft.Maui.Platform;

#endif

namespace RTCChat.Managers
{
    public static class AdvancedColorManager
    {
		public static void SetBarsColor(Color color)
		{
#if ANDROID
			var window = (Application.Current?.Windows[0]?.Handler?.PlatformView as Android.App.Activity)?.Window;
			if (window == null) return;

			// Устанавливаем цвет
			window.SetStatusBarColor(color.ToPlatform());
			window.SetNavigationBarColor(color.ToPlatform());

			// Иконки светлые или тёмные
			var decor = window.DecorView;
			//decor.SystemUiFlags = darkIcons  SystemUiFlags.LightStatusBar;

			if(color.GetLuminosity() > 0.5f) decor.SystemUiFlags |= SystemUiFlags.LightStatusBar;
			else decor.SystemUiFlags &= ~SystemUiFlags.LightStatusBar;

			//if (App.Current.RequestedTheme == AppTheme.Light)
			//	decor.SystemUiFlags |= SystemUiFlags.LightStatusBar;
			//else
			//	decor.SystemUiFlags &= ~SystemUiFlags.LightStatusBar;
#endif
		}
	}
}
