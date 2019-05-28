using ByteSizeLib;
using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        string rutaInicio = "";
        public TcpListener listener;
        public TcpClient tcpClient;
        public Thread hiloRecibos;
        public bool parar = false;
        InterfazCodigoEspecifico interfazCodigo = DependencyService.Get<InterfazCodigoEspecifico>();
        public MainPage()
        {
            InitializeComponent();

            this.BackgroundColor = Color.LightBlue;
            Listado.SeparatorColor = Color.Black;
            cargaLista();

            listener = new TcpListener(IPAddress.Any, App.paginaInicio.puertoArchivos);
            listener.Start();

            hiloRecibos = new Thread(() => descargaArchivo());
            hiloRecibos.IsBackground = true;
            hiloRecibos.Start();

            fabDescargar.Clicked += CompruebaEspacioDescarga;

            NavigationPage.SetHasBackButton(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void CompruebaEspacioDescarga(object sender, EventArgs e)
        {
            App.espacioLibre = await interfazCodigo.espacioLibre(App.lugarDescargaId);
            DescargarMsg();
        }

        private void cargaLista()
        {
            try
            {
                App.sw.WriteLine("LISTADO");
                App.sw.Flush();
                string nombres = App.sr.ReadLine();

                lista.Clear();
                string[] nombreSplit = nombres.Split('?');
                foreach (string nombre in nombreSplit)
                {
                    if (nombre != "")
                        lista.Add(new ElementoLista(nombre));
                }
                if (rutaInicio != "")
                    lista.Insert(0, new ElementoLista("O:..."));
                lista.ElementAt(0).EsCarpeta = true;
                Listado.ItemsSource = null;
                Listado.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                App.paginaInicio.errorPerdidaConexion();
                return;
            }
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

        private void FabAjustes_Clicked(object sender, EventArgs e)
        {
            App.paginaConfiguracion = new PaginaConfiguracion();
            ((NavigationPage)this.Parent).PushAsync(App.paginaConfiguracion);
        }

        private void FabUsuario_Clicked(object sender, EventArgs e)
        {
            App.configUsuario = new ConfigUsuario();
            ((NavigationPage)this.Parent).PushAsync(App.configUsuario);
        }

        private void Listado_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                ElementoLista elemento = (ElementoLista)e.SelectedItem;
                if (elemento.EsCarpeta && elemento.Nombre != "...")
                {
                    App.sw.WriteLine("DIRECTORIO " + elemento.Nombre);
                    App.sw.Flush();
                    string msg = App.sr.ReadLine();
                    if (msg == "Valido")
                    {
                        rutaInicio += '?' + elemento.Nombre;
                        cargaLista();
                    }
                }
                else
                {
                    if (elemento.Nombre == "...")
                    {
                        App.sw.WriteLine("ATRAS");
                        App.sw.Flush();
                        rutaInicio = rutaInicio.Substring(0, rutaInicio.LastIndexOf('?'));
                        cargaLista();
                    }
                }
            }
            catch (Exception ex)
            {
                App.paginaInicio.errorPerdidaConexion();
                return;
            }
        }

        private void DescargarMsg()
        {
            try
            {
                ElementoLista elementoSeleccionado = (ElementoLista)Listado.SelectedItem;
                if (elementoSeleccionado == null)
                {
                    DisplayAlert("Atención", "Debes seleccionar un elemento", "OK");
                }
                else
                {
                    if (elementoSeleccionado.EsCarpeta)
                        DisplayAlert("Atención", "Debes seleccionar un elemento", "OK");
                    else
                    {
                        ByteSize tamañoFichero = new ByteSize(elementoSeleccionado.Tamaño);
                        if (App.espacioLibre > elementoSeleccionado.Tamaño || (Device.RuntimePlatform == "UWP" && App.lugarDescargaId == 0))
                        {
                            App.sw.WriteLine("FICHERO " + elementoSeleccionado.Nombre);
                            App.sw.Flush();
                            DisplayAlert("Atención", "El fichero ocupa " + tamañoFichero.ToString(), "OK");
                        }
                        else
                        {
                            DisplayAlert("Atención", "El fichero ocupa " + tamañoFichero.ToString() + " y no hay espacio suficiente", "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.paginaInicio.errorPerdidaConexion();
                return;
            }
        }

        private void descargaArchivo()
        {
            while (!parar)
            {
                if (listener != null)
                    if (listener.Pending())
                    {
                        tcpClient = listener.AcceptTcpClient();
                       
                        ElementoLista elementoSeleccionado = (ElementoLista)Listado.SelectedItem;

                        switch (interfazCodigo.GuardarFichero(elementoSeleccionado.Nombre, tcpClient.GetStream(), App.lugarDescargaId).Result)
                        {
                            case 'C':
                                if (App.notificaciones)
                                    CrossLocalNotifications.Current.Show("ClienteFTP", "Fichero Creado");
                                if (App.dialogos)
                                    Device.BeginInvokeOnMainThread(new Action(MensajeCreado));
                                break;
                            case 'N':
                                if (App.notificaciones)
                                    CrossLocalNotifications.Current.Show("ClienteFTP", "No se ha podido descargar el ichero");
                                if (App.dialogos)
                                    Device.BeginInvokeOnMainThread(new Action(MensajeNoCreado));
                                break;
                            case 'E':
                                if (App.notificaciones)
                                    CrossLocalNotifications.Current.Show("ClienteFTP", "El fichero ya existe");
                                if (App.dialogos)
                                    Device.BeginInvokeOnMainThread(new Action(MensajeExiste));
                                break;
                        }
                    }
            }
        }

        private void MensajeCreado()
        {
            DisplayAlert("Atención", "Fichero Creado", "OK");
        }
        private void MensajeNoCreado()
        {
            DisplayAlert("Atención", "No se ha podido crear", "OK");
        }
        private void MensajeExiste()
        {
            DisplayAlert("Atención", "El fichero ya existe", "OK");
        }
    }
}
