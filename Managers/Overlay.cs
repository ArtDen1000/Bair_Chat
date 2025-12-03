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
        public virtual void CancelOverlay() => IsOverlay = false;
		[RelayCommand]
		public virtual void ShowOverlay() => IsOverlay = true;
		[RelayCommand]
		public virtual void ToggleOverlay() => IsOverlay = !IsOverlay;

	}
}
