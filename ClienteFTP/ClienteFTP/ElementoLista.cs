using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ClienteFTP
{
    class ElementoLista
    {
       private string imagen;
        string nombre;
        char tipo;
        public string Imagen { get => imagen; set => imagen = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public char Tipo { get => tipo; set => tipo = value; }

        public ElementoLista(string nombreCompleto)
        {
            if (nombreCompleto.StartsWith("D:"))
                Imagen = "folder.png";
            else
                Imagen = "file.png";
            Nombre = nombreCompleto.Substring(2);
        }

    }
}
