using ByteSizeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClienteFTP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaConfiguracion : ContentPage
    {
        InterfazCodigoEspecifico interfazCodigo = DependencyService.Get<InterfazCodigoEspecifico>();
        public PaginaConfiguracion()
        {
            InitializeComponent();
            chkNotificaciones.IsToggled = App.notificaciones;
            this.BackgroundColor = Color.LightBlue;

            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    cboLugarDescarga.Items.Add("Descargas");
                    break;
                case Device.UWP:
                    cboLugarDescarga.Items.Add("Descargas");
                    cboLugarDescarga.Items.Add("Librería de Musica");
                    cboLugarDescarga.Items.Add("Librería de Videos");
                    break;
            }
            cboLugarDescarga.SelectedIndex = App.lugarDescargaId;

            cboLugarDescarga.SelectedIndexChanged += cargaDatosEspacio;

            btnApagarServer.IsVisible = App.esAdmin;
            btnApagarServer.Clicked += async (sender, e) => await apagarServer();

            chkAuto.IsToggled = Preferences.Get("auto", false);
        }

        private void ChkNotificaciones_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("notificaciones", chkNotificaciones.IsToggled);
            App.notificaciones = chkNotificaciones.IsToggled;
            if (chkNotificaciones.IsToggled)
                chkDialogos.IsToggled = false;
        }

        private void CboLugarDescarga_SelectedIndexChanged(object sender, EventArgs e)
        {
            Preferences.Set("lugarDescargaId", cboLugarDescarga.SelectedIndex);
            App.lugarDescargaId = cboLugarDescarga.SelectedIndex;

        }
        private async void cargaDatosEspacio(object sender, EventArgs e)
        {
            ByteSize libre = new ByteSize(await interfazCodigo.espacioLibre(cboLugarDescarga.SelectedIndex));
            ByteSize total = new ByteSize(await interfazCodigo.espacioTotal(cboLugarDescarga.SelectedIndex));
            string cadena = String.Format("{0:0.00}GB/{1:0.00}GB {2:0.0}%", libre.GigaBytes, total.GigaBytes, (total.GigaBytes - libre.GigaBytes) / total.GigaBytes * 100);
            txtEspacio.Text = cadena;
        }

        private async Task apagarServer()
        {
            try
            {
                var resultado = await DisplayAlert("Atención", "¿Desea apagar el servidor?", "Sí", "No");
                if (resultado)
                {
                    App.sw.WriteLine("CERRAR");
                    App.sw.Flush();

                    await DisplayAlert("Atención", "Se ha apagado correctamente", "OK");

                    if (App.paginaInicio != null)
                        App.paginaInicio = new PaginaInicio();
                    await ((NavigationPage)this.Parent).PushAsync(App.paginaInicio);
                }
            }
            catch (Exception ex)
            {
                App.paginaInicio.errorPerdidaConexion();
                return;
            }
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            cargaDatosEspacio(this, new EventArgs());
        }

        private void ChkDialogos_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("dialogos", chkDialogos.IsToggled);
            App.dialogos = chkDialogos.IsToggled;
            if (chkDialogos.IsToggled)
                chkNotificaciones.IsToggled = false;
        }

        private void ChkAuto_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("auto", chkAuto.IsToggled);
        }
    }
}