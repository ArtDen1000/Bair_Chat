using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RTCChat.Managers
{
    public partial class Overlay : ObservableObject
	{
		[ObservableProperty] public bool isOverlay = false;

		[RelayCommand]
        public virtual void CloseOverlay()
		{
			overlayStack.RemoveAt(overlayStack.Count - 1);
			if(overlayStack.Count == 0)
			{
				IsOverlay = false;
			}
			else SetOverlay(overlayStack.Last());
		}
		[RelayCommand]
		public virtual void ShowOverlay() => IsOverlay = true;
		[RelayCommand]
		public virtual void ToggleOverlay() => IsOverlay = !IsOverlay;
		
		private List<View> overlayStack = new List<View>();
		private Border element;
		public void SetOrigin(Border element) => this.element = element;

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
