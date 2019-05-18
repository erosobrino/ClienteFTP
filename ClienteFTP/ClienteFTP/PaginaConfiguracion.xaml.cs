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
        public Dictionary<string, System.Environment.SpecialFolder> lugarDescarga = new Dictionary<string, System.Environment.SpecialFolder>();
        private const long Kb = 1024;
        private const long Mb = Kb * 1024;
        private const long Gb = Mb * 1024;
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

            btnApagarServer.IsVisible = App.esAdmin;
            btnApagarServer.Clicked += async (sender, e) => await apagarServer();
        }

        private void ChkNotificaciones_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("notificaciones", chkNotificaciones.IsToggled);
            App.notificaciones = chkNotificaciones.IsToggled;
        }

        private void CboLugarDescarga_SelectedIndexChanged(object sender, EventArgs e)
        {
            Preferences.Set("lugarDescargaId", cboLugarDescarga.SelectedIndex);
            App.lugarDescargaId = cboLugarDescarga.SelectedIndex;
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
    }
}