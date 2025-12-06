using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Behaviors;

namespace RTCChat.Behaviors
{
    public class ImageTintColorBehavior : Behavior<ImageButton>
    {
		//public static readonly BindableProperty ColorResourceProperty =
		//BindableProperty.Create(
		//	nameof(ColorResource),
		//	typeof(Color),
		//	typeof(ImageTintColorBehavior),
		//	App.Current.Resources["Contrast1"] as Color,
		//	propertyChanged: OnColorChanged);

		//public Color ColorResource
		//{
		//	get => (Color)GetValue(ColorResourceProperty);
		//	set => SetValue(ColorResourceProperty, value);
		//}

		//private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
		//{
		//	if (newValue is Color color)
		//	{
		//		List<Behavior> behaviors = (bindable as ImageButton).Behaviors.Where(p => p.GetType() != typeof(IconTintColorBehavior)).ToList();
		//		behaviors.Add(new IconTintColorBehavior()
		//		{
		//			TintColor = App.Current.Resources["ContrastCustom"] as Color
		//		});

		//		(bindable as ImageButton).Behaviors.Clear();
		//		//foreach(var b in behaviors)
		//		//	(bindable as ImageButton).Behaviors.Add(b);
		//	}
		//}




		protected override void OnAttachedTo(ImageButton bindable)
		{
			base.OnAttachedTo(bindable);

			// Первичная установка цвета
			foreach(var b in bindable.Behaviors)
			{
				if(b is IconTintColorBehavior) return;
			}
			bindable.Behaviors.Add(new IconTintColorBehavior()
			{
				TintColor = App.Current.Resources["ContrastCustom"] as Color
			});
		}

		protected override void OnDetachingFrom(ImageButton bindable)
		{
			base.OnDetachingFrom(bindable);
		}
	}
}
