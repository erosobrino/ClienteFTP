using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClienteFTP
{
    public interface Guardado
    {
        Task<char> GuardarFichero(string nombre, NetworkStream stream, int idCarpeta);
    }
}
