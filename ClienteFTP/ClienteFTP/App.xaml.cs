using System;
using System.IO;
using System.Net.Sockets;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClienteFTP
{
    public partial class App : Application
    {
        public static NetworkStream ns = null;
        public static StreamReader sr = null;
        public static StreamWriter sw = null;
        public static Socket sServer = null;

        public static PaginaInicio paginaInicio = null;
        public static MainPage mainPage = null;
        public static PaginaConfiguracion paginaConfiguracion = null;
        public static ConfigUsuario configUsuario = null;
        public static PaginaInfo paginaInfo = null;
        public static PaginaUsuarios paginaUsuarios = null;
        public static PaginaModificacionUsuario paginaModificacionUsuario = null;

        public static string nombre;
        public static string pass;
        public static int lugarDescargaId;
        public static bool esAdmin = false;
        public static long espacioLibre;

        public static bool notificaciones;
        public static bool dialogos;
        public App()
        {
            InitializeComponent();

            notificaciones = Preferences.Get("notificaciones", true);
            lugarDescargaId = Preferences.Get("lugarDescargaId", 0);
            dialogos = Preferences.Get("dialogos", false);

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
