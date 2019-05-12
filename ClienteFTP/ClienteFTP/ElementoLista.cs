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
        private char tipo;
        private bool esCarpeta = false;
        public string Imagen { get => imagen; set => imagen = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public char Tipo { get => tipo; set => tipo = value; }
        public bool EsCarpeta { get => esCarpeta; set => esCarpeta = value; }

        public ElementoLista(string nombreCompleto)
        {
            if (nombreCompleto.StartsWith("D:"))
            {
                Imagen = "folder.png";
                EsCarpeta = true;
            }
            else
            {
                if (nombreCompleto.StartsWith("F:"))
                {
                    Imagen = "file.png";
                    EsCarpeta = false;
                }
            }
            Nombre = nombreCompleto.Substring(2);
        }

    }
}
