using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClienteFTP
{
    public interface Guardado
    {
        Task GuardarFichero(string nombre,string texto,int idCarpeta);
    }
}
