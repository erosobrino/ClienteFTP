using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClienteFTP
{
    [DesignTimeVisible(true)]
    public partial class PaginaInicio : ContentPage
    {
        IPEndPoint ie;
        IPAddress ip;
        int puerto;
        public int puertoArchivos;
        public PaginaInicio()
        {
            InitializeComponent();

            this.BackgroundColor = Color.LightBlue;

            chkDatos.IsToggled = Preferences.Get("recordar", false);
            if (chkDatos.IsToggled)
            {
                txtIP.Text = Preferences.Get("ip", "");
                txtPuerto.Text = Preferences.Get("puerto", "");
                txtPuertoArchivos.Text = Preferences.Get("puertoArchivos", "");
                txtUsuario.Text = Preferences.Get("usuario", "");
                txtContraseña.Text = Preferences.Get("contraseña", "");
                chkAuto.IsToggled = Preferences.Get("auto", false);
            }
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
            if (txtPuertoArchivos.Text == "")
            {
                DisplayAlert("Atención", "Debes introducir el Puerto de archivos", "OK");
                txtPuertoArchivos.Focus();
                return;
            }
            else
            {
                puertoArchivos = Convert.ToInt32(txtPuertoArchivos.Text);
                if (puertoArchivos < 0 || puertoArchivos > IPEndPoint.MaxPort)
                {
                    DisplayAlert("Atención", "Debes introducir un puerto de archivos válido", "OK");
                    txtPuertoArchivos.Focus();
                    return;
                }
            }

            if (txtPuerto.Text == txtPuertoArchivos.Text)
            {
                DisplayAlert("Atención", "Los puertos tienen que ser distintos", "OK");
                txtPuerto.Focus();
                return;
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

            if (chkDatos.IsToggled)
            {
                Preferences.Set("recordar", true);
                Preferences.Set("ip", txtIP.Text);
                Preferences.Set("puerto", txtPuerto.Text);
                Preferences.Set("puertoArchivos", txtPuertoArchivos.Text);
                Preferences.Set("usuario", txtUsuario.Text);
                Preferences.Set("contraseña", txtContraseña.Text);
                Preferences.Set("auto", chkAuto.IsToggled);
            }
            ie = new IPEndPoint(ip.Address, puerto);
            App.sServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                App.sServer.Connect(ie);
                App.ns = new NetworkStream(App.sServer);
                App.sr = new StreamReader(App.ns);
                App.sw = new StreamWriter(App.ns);
                App.sr.ReadLine();
                App.sw.WriteLine("=" + txtUsuario.Text + " =" + txtContraseña.Text);
                App.sw.Flush();
                if (App.sr.ReadLine() == "valido")
                {
                    App.nombre = txtUsuario.Text;
                    App.pass = txtContraseña.Text;

                    App.esAdmin = true;
                    //NuevoMetodo en server pedir admin

                    App.mainPage = new MainPage();
                    ((NavigationPage)this.Parent).PushAsync(App.mainPage);
                }
                else
                    DisplayAlert("Atención", "Tus credenciales No son válidos", "OK");
            }
            catch (Exception)
            {
                DisplayAlert("Atención", "No se ha podido establecer conexión con el servidor", "OK");
            }
        }

        private void ChkDatos_Toggled(object sender, ToggledEventArgs e)
        {
            if (chkDatos.IsToggled)
                chkAuto.IsEnabled = true;
            else
            {
                chkAuto.IsEnabled = false;
                chkAuto.IsToggled = false;
            }
        }

        private void TxtIP_Completed(object sender, EventArgs e)
        {
            txtPuerto.Focus();
            txtPuerto.CursorPosition = txtPuerto.Text.Length;
        }

        private void TxtPuerto_Completed(object sender, EventArgs e)
        {
            txtPuertoArchivos.Focus();
            txtPuertoArchivos.CursorPosition = txtPuertoArchivos.Text.Length;
        }

        private void TxtUsuario_Completed(object sender, EventArgs e)
        {
            txtContraseña.Focus();
            txtContraseña.CursorPosition = txtContraseña.Text.Length;
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            if (chkAuto.IsToggled)
                BtnConectar_Clicked(this, new EventArgs());
        }

        public void errorPerdidaConexion()
        {
            //TODO poder reconectarse
            DisplayAlert("Atención", "Se ha perdido la conexión, Conectate de nuevo", "OK");
            App.sw.Close();
            App.sr.Close();       
            App.ns.Close();
            App.sServer.Close();
            App.paginaConfiguracion = null;
            App.mainPage = null;
            App.paginaInfo = null;
            ((NavigationPage)this.Parent).PopToRootAsync();
        }

        private void TxtPuertoArchivos_Completed(object sender, EventArgs e)
        {
            txtUsuario.Focus();
            txtUsuario.CursorPosition = txtUsuario.Text.Length;
        }
    }
}