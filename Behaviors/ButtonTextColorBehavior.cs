using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCChat.Behaviors
{
    public class ButtonTextColorBehavior : Behavior<Button>
    {
		private void ColorChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Button.BackgroundColor))
			{
				SetColor((Button)sender);
			}
		}

		private void SetColor(Button button)
		{
			button.TextColor = button.BackgroundColor.GetLuminosity() > 0.4f ? App.Current.Resources["Black"] as Color : App.Current.Resources["White"] as Color;
		}

		protected override void OnAttachedTo(Button bindable)
		{
			base.OnAttachedTo(bindable);
			bindable.PropertyChanged += ColorChanged;

			// Первичная установка цвета
			SetColor(bindable);
		}

		protected override void OnDetachingFrom(Button bindable)
		{
			base.OnDetachingFrom(bindable);
		}
	}
}
