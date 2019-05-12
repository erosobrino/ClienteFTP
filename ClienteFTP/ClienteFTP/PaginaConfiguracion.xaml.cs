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

            //lugarDescarga.Add("Almacenamiento Interno", System.Environment.SpecialFolder.MyDocuments);
            //lugarDescarga.Add("Almacenamiento Externo", System.Environment.SpecialFolder.MyComputer);

            //foreach (string nombre in lugarDescarga.Keys)
            //{
            //    cboLugarDescarga.Items.Add(nombre);
            //}

            //DriveInfo[] allDrives = DriveInfo.GetDrives();

            //foreach (DriveInfo d in allDrives)
            //{
            //    Console.WriteLine(d.RootDirectory);
            //    Console.WriteLine("Drive {0}", d.Name);
            //    Console.WriteLine("  Drive type: {0}", d.DriveType);

            //    if (d.IsReady == true)
            //    {
            //        Console.WriteLine("Volume label: {0}", d.VolumeLabel);
            //        Console.WriteLine("File system: {0}", d.DriveFormat);
            //        Console.WriteLine("Available space to current user:{0, 15} ", ByteSize.FromBytes(d.AvailableFreeSpace));
            //        Console.WriteLine("Total size of drive: {0, 15} bytes ", ByteSize.FromBytes(d.TotalSize));
            //    }
            //}

            cboLugarDescarga.SelectedIndex = App.lugarDescargaId;
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
    }
}