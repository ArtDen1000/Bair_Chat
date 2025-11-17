using RTCChat.MVVM;

namespace RTCChat
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MenuVM(this);
        }

    }
}
