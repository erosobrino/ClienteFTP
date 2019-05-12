using System;
using System.IO;
using Windows.Storage;
using Windows.UI.Popups;

namespace ClienteFTP.UWP
{
    class GuardarWindows : Guardado
    {
        public async System.Threading.Tasks.Task GuardarFichero(string nombre, string texto, int idCarpeta)
        {
            try
            {
                StorageFile fichero = null;
                switch (idCarpeta)
                {
                    case 0:
                        fichero = await DownloadsFolder.CreateFileAsync(nombre, CreationCollisionOption.FailIfExists);
                        break;
                    case 1:
                        fichero = await KnownFolders.MusicLibrary.CreateFileAsync(nombre, CreationCollisionOption.FailIfExists);
                        break;
                    case 2:
                        fichero = await KnownFolders.VideosLibrary.CreateFileAsync(nombre, CreationCollisionOption.FailIfExists);
                        break;
                }
                if (fichero != null)
                {
                    using (Stream str = await fichero.OpenStreamForWriteAsync())
                    {
                        using (StreamWriter streamWriter = new StreamWriter(str))
                        {
                            streamWriter.WriteLine(texto);
                        }
                    }
                }
                MessageDialog mensaje = new MessageDialog("Fichero Creado");
                await mensaje.ShowAsync();
            }
            catch (Exception)
            {
                MessageDialog mensaje = new MessageDialog("Ya existe el fichero");
                await mensaje.ShowAsync();
            }
        }
    }
}
