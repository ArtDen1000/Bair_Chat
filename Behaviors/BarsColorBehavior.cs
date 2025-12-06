using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTCChat.Managers;

namespace RTCChat.Behaviors
{
    public class BarsColorBehavior : Behavior<ContentPage>
    {
		private void ColorChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Page.BackgroundColor))
			{
				SetColor(((Page)sender).BackgroundColor);
			}
		}

		private void SetColor(Color color)
		{
			AdvancedColorManager.SetBarsColor(color);
		}

		protected override void OnAttachedTo(ContentPage bindable)
		{
			base.OnAttachedTo(bindable);
			bindable.PropertyChanged += ColorChanged;

			// Первичная установка цвета
			SetColor(bindable.BackgroundColor);
		}

		protected override void OnDetachingFrom(ContentPage bindable)
		{
			base.OnDetachingFrom(bindable);
		}
	}
}
