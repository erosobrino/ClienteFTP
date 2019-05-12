using ClienteFTP.UWP;
using Xamarin.Forms;

[assembly: Dependency(typeof(GuardarWindows))]
namespace ClienteFTP.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            LoadApplication(new ClienteFTP.App());
        }
    }
}
