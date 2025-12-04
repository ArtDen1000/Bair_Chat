using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCChat.Behaviors
{
    public class LabelSizeBehavior : Behavior<Label>
    {
		protected override void OnAttachedTo(Label bindable)
		{
			base.OnAttachedTo(bindable);
			bindable.ParentChanged += OnSizeChanged;
			OnSizeChanged(bindable, EventArgs.Empty);
		}

		protected override void OnDetachingFrom(Label bindable)
		{
			bindable.ParentChanged -= OnSizeChanged;
			base.OnDetachingFrom(bindable);
		}

		private void OnSizeChanged(object sender, EventArgs e)
		{
			var label = (Label)sender;

			if (label.Width > 245)
				label.WidthRequest = 250;
		}
	}
}
