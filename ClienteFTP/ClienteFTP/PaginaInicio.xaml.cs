using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClienteFTP
{
    [DesignTimeVisible(true)]
    public partial class PaginaInicio : ContentPage
    {
        IPEndPoint ie;
        Socket sServer;
        IPAddress ip;
        int puerto;
        public PaginaInicio()
        {
            InitializeComponent();
            this.BackgroundColor = Color.LightBlue;
        }

        private void BtnConectar_Clicked(object sender, EventArgs e)
        {
            if (txtIP.Text == "")
            {
                DisplayAlert("Atención", "Debes introducir la IP", "OK");
                txtIP.Focus();
                return;
            }
            else
            {
                if (!IPAddress.TryParse(txtIP.Text, out ip))
                {
                    DisplayAlert("Atención", "La IP no es válida", "OK");
                    txtIP.Focus();
                    return;
                }
            }

            if (txtPuerto.Text == "")
            {
                DisplayAlert("Atención", "Debes introducir el Puerto", "OK");
                txtPuerto.Focus();
                return;
            }
            else
            {
                puerto = Convert.ToInt32(txtPuerto.Text);
                if (puerto < 0 || puerto > IPEndPoint.MaxPort)
                {
                    DisplayAlert("Atención", "Debes introducir un puerto válido", "OK");
                    txtPuerto.Focus();
                    return;
                }
            }
            if (txtUsuario.Text == "")
            {
                DisplayAlert("Atención", "Debes introducir el Usuario", "OK");
                txtUsuario.Focus();
                return;
            }
            if (txtContraseña.Text == "")
            {
                DisplayAlert("Atención", "Debes introducir la Contraseña", "OK");
                txtContraseña.Focus();
                return;
            }

            ie = new IPEndPoint(ip.Address, puerto);
            sServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sServer.Connect(ie);
                using (App.ns = new NetworkStream(sServer))
                {
                    using (App.sr = new StreamReader(App.ns))
                    {
                        using (App.sw = new StreamWriter(App.ns))
                        {
                            App.sr.ReadLine();
                            App.sw.WriteLine("=" + txtUsuario.Text + " =" + txtContraseña.Text);
                            App.sw.Flush();
                            if (App.sr.ReadLine() == "valido")
                            {
                                App.mainPage = new MainPage();
                            }
                            else
                                DisplayAlert("Atención", "Tus credenciales No son válidos", "OK");
                        }
                    }
                }

            }
            catch (IOException)
            {
                DisplayAlert("Atención", "No se ha podido establecer conexión con el servidor", "OK");
            }
        }
    }
}