using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ClienteFTP
{
    class ElementoLista
    {
        private string imagen;
        private string nombre;
        private bool esCarpeta = false;
        private int tamaño;
        public string Imagen { get => imagen; set => imagen = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public bool EsCarpeta { get => esCarpeta; set => esCarpeta = value; }
        public int Tamaño { get => tamaño; set => tamaño = value; }

        public ElementoLista(string nombreCompleto)
        {
            if (nombreCompleto.StartsWith("D:"))
            {
                Imagen = "folder.png";
                EsCarpeta = true;
                Nombre = nombreCompleto.Substring(2);
            }
            else
            {
                if (nombreCompleto.StartsWith("F:"))
                {
                    Imagen = "file.png";
                    EsCarpeta = false;
                }
                Nombre = nombreCompleto.Substring(2, nombreCompleto.IndexOf('#')-2);
                Tamaño = Convert.ToInt32(nombreCompleto.Substring(nombreCompleto.IndexOf('#')+1));
            }
        }

    }
}
