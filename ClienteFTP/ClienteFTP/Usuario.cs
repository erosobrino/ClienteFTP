using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteFTP
{
    public class Usuario
    {
        private int id;
        private string nombre;
        private string contraseña;
        private bool esAdmin;

        public bool EsAdmin { get => esAdmin; set => esAdmin = value; }
        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Contraseña { get => contraseña; set => contraseña = value; }

        public Usuario(string datosUnidos)
        {
            string[] datosSeparados = datosUnidos.Split('|');
            Id = Convert.ToInt32(datosSeparados[0]);
            Nombre = datosSeparados[1];
            Contraseña = datosSeparados[2];
            EsAdmin = Boolean.Parse(datosSeparados[3]);
        }
    }
}
