using System;
using System.IO;
using System.Net.Sockets;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClienteFTP
{
    public partial class App : Application
    {
        public static NetworkStream ns = null;
        public static StreamReader sr = null;
        public static StreamWriter sw = null;

        public PaginaInicio paginaInicio = null;
        public static MainPage mainPage = null;
        public App()
        {
            InitializeComponent();
            paginaInicio = new PaginaInicio();
            MainPage = new NavigationPage(paginaInicio);
            //MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
