using System;
using ClienteFTP.UWP;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Xamarin.Forms;

[assembly: Dependency(typeof(InterfazCodigoEspecificoWindows))]
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
