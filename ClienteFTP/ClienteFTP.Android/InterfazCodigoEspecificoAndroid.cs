using Android;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Java.IO;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ClienteFTP.Droid
{
    class InterfazCodigoEspecificoAndroid : InterfazCodigoEspecifico
    {
        public async Task<long> espacioLibre(int idCarpeta)
        {
            return Android.OS.Environment.RootDirectory.UsableSpace;
        }

        public async Task<long> espacioTotal(int idCarpeta)
        {
            return Android.OS.Environment.RootDirectory.TotalSpace;
        }

        public async Task<char> GuardarFichero(string nombre, NetworkStream strIn, int idCarpeta)
        {
            try
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath);
                Java.IO.File file = new Java.IO.File(dir, nombre);
                if (!file.Exists())
                {

                    file.CreateNewFile();
                    FileOutputStream writer = new FileOutputStream(file);
                    var buffer = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = strIn.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        writer.Write(buffer, 0, bytesRead);
                    }
                    writer.Flush();
                    writer.Close();
                    return 'C';
                }
                else
                    return 'E';
            }
            catch
            {
                return 'N';
            }
        }
    }
}