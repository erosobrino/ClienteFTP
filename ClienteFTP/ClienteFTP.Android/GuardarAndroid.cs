using Android;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Java.IO;
using System;
using System.Threading.Tasks;

namespace ClienteFTP.Droid
{
    class GuardarAndroid : Guardado
    {
        public async Task GuardarFichero(string nombre, string texto, int idCarpeta)
        {
            try
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath);
                Java.IO.File file = new Java.IO.File(dir, "iootext.txt");
                if (!file.Exists())
                {

                    file.CreateNewFile();
                    FileWriter writer = new FileWriter(file);
                    // Writes the content to the file
                    writer.Write(texto);
                    writer.Flush();
                    writer.Close();
                    await App.Current.MainPage.DisplayAlert("Atención", "Fichero Creado", "OK");
                }
                else
                    await App.Current.MainPage.DisplayAlert("Atención", "Ya existe el fichero", "OK");
            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Atención", "Ya existe el fichero", "OK");
            }
        }
    }
}