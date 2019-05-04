using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClienteFTP
{
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        List<ElementoLista> lista = new List<ElementoLista>();
        List<ElementoLista> listaVisible = new List<ElementoLista>();
        bool menuPulsado = false;
        public MainPage()
        {
            InitializeComponent();

            lista.Add(new ElementoLista("F:dfasffdgdf"));
            lista.Add(new ElementoLista("D:sdafsfsf"));
            lista.Add(new ElementoLista("F:sdsdafsfsf"));

            this.BackgroundColor = Color.LightBlue;
            Listado.ItemsSource = lista;
            Listado.SeparatorColor = Color.Black;
        }

        private void TxtBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            Listado.ItemsSource = null;
            if (txtBusqueda.Text != "")
            {
                listaVisible.Clear();
                foreach (ElementoLista elemento in lista)
                {
                    if (elemento.Nombre.Contains(txtBusqueda.Text))
                        listaVisible.Add(elemento);
                }
                Listado.ItemsSource = listaVisible;
            }
            else
                Listado.ItemsSource = lista;
        }

        private void FabVermas_Clicked(object sender, EventArgs e)
        {
            menuPulsado = !menuPulsado;
            if (menuPulsado)
                fabVerMas.Source = "fab2.png";
            else
                fabVerMas.Source = "fab1.png";

            fabDescargar.IsVisible = menuPulsado;
            fabUsuario.IsVisible = menuPulsado;
            fabAjustes.IsVisible = menuPulsado;
        }

        private void FabDescargar_Clicked(object sender, EventArgs e)
        {

        }

        private void FabAjustes_Clicked(object sender, EventArgs e)
        {

        }
        private void FabUsuario_Clicked(object sender, EventArgs e)
        {

        }
    }
}
