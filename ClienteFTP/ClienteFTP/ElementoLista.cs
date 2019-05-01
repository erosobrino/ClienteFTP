using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ClienteFTP
{
    class ElementoLista
    {
       private Image imagen;
        string nombre;
        char tipo;
        public Image Imagen { get => imagen; set => imagen = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public char Tipo { get => tipo; set => tipo = value; }

        public ElementoLista()
        {
            Nombre = "hola";
        }

    }
}
