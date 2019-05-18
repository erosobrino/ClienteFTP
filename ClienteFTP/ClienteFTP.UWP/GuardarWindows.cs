using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace ClienteFTP.UWP
{
    class GuardarWindows : Guardado
    {
        public async Task<char> GuardarFichero(string nombre, NetworkStream strIn, int idCarpeta)
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
                        try
                        {
                            StorageFile file = await KnownFolders.MusicLibrary.GetFileAsync(nombre);
                        }
                        catch
                        {
                            return 'E';
                        }
                        fichero = await KnownFolders.MusicLibrary.CreateFileAsync(nombre, CreationCollisionOption.FailIfExists);
                        break;
                    case 2:
                        try
                        {
                            StorageFile file = await KnownFolders.VideosLibrary.GetFileAsync(nombre);
                        }
                        catch
                        {
                            return 'E';
                        }
                        fichero = await KnownFolders.VideosLibrary.CreateFileAsync(nombre, CreationCollisionOption.FailIfExists);
                        break;
                }
                if (fichero != null)
                {
                    using (Stream strOut = await fichero.OpenStreamForWriteAsync())
                    {
                        var buffer = new byte[1024];
                        int bytesRead;
                        while ((bytesRead = strIn.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            strOut.Write(buffer, 0, bytesRead);
                        }
                    }
                    return 'C';
                }
                return 'N';
            }
            catch (Exception)
            {
                if (idCarpeta == 0)
                {
                    StorageFile fichero = await DownloadsFolder.CreateFileAsync(nombre, CreationCollisionOption.OpenIfExists);
                    if (fichero != null)
                        return 'E';
                    else
                        return 'N';
                }
                else
                    return 'N';
            }
        }
    }
}
