using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RTCChat.Managers
{
    public class Overlay
	{
		private List<View> overlayStack = new List<View>();
		private Border element;

		public delegate void OverlayVisible(bool isVisible);
		public event OverlayVisible OverlayVisibleChanged;

		public bool CloseOverlay()
		{
			if(overlayStack.Count == 0) return false;
			overlayStack.RemoveAt(overlayStack.Count - 1);
			if(overlayStack.Count == 0)
			{
				OverlayVisibleChanged.Invoke(false);
			}
			else SetOverlay(overlayStack.Last());

			return true;
		}
		public void ShowOverlay() => OverlayVisibleChanged.Invoke(true);

		public void SetOrigin(Border element)
		{
			this.element = element;
		}

		public void SetOverlay(View content)
		{
			element.Content = content;
		}

		public void AddOverlay(View content)
		{
			overlayStack.Add(content);
			SetOverlay(overlayStack.Last());
		}

	}
}
