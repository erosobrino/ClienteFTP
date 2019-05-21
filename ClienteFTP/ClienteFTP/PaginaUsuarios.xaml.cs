using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClienteFTP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaUsuarios : ContentPage
    {
        List<Usuario> usuarios = new List<Usuario>();
        public Usuario usuarioActual = null;
        public PaginaUsuarios()
        {
            InitializeComponent();

            this.BackgroundColor = Color.LightBlue;

            this.Appearing += PaginaUsuarios_Appearing;

        }

        private void PaginaUsuarios_Appearing(object sender, EventArgs e)
        {
            try
            {
                usuarios.Clear();
                App.sw.WriteLine("LISTAUSUARIOS");
                App.sw.Flush();
                string listadoNombres = App.sr.ReadLine();
                string[] nombresStr = listadoNombres.Split('#');
                foreach (string nombreStr in nombresStr)
                {
                    if (nombreStr.Length > 0)
                        usuarios.Add(new Usuario(nombreStr));
                }
                ListadoUsuarios.ItemsSource = null;
                ListadoUsuarios.ItemsSource = usuarios;
            }
            catch
            {
                App.paginaInicio.errorPerdidaConexion();
            }
        }

        private void BtnModificar_Clicked(object sender, EventArgs e)
        {
            foreach (Usuario usuario in usuarios)
            {
                if (usuario.Id == Convert.ToInt32(((ImageButton)sender).CommandParameter))
                    usuarioActual = usuario;
            }
            App.paginaModificacionUsuario = new PaginaModificacionUsuario();
            App.paginaModificacionUsuario.id = usuarioActual.Id;
            ((NavigationPage)this.Parent).PushAsync(App.paginaModificacionUsuario);
        }

        private void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            try
            {
                usuarioActual = (Usuario)ListadoUsuarios.SelectedItem;
                App.sw.WriteLine("ELIMINAR " + usuarioActual.Id);
                App.sw.Flush();
                string msg = App.sr.ReadLine();
                if (msg == "valido")
                {
                    DisplayAlert("Atención", "Usuario " + usuarioActual.Nombre + " eliminado", "OK");
                    usuarios.Remove(usuarioActual);
                }
                else
                    DisplayAlert("Atención", "No se ha podido eliminar a " + usuarioActual.Nombre, "OK");
            }
            catch
            {
                App.paginaInicio.errorPerdidaConexion();
            }
        }

        private void FabAdd_Clicked(object sender, EventArgs e)
        {
            App.paginaModificacionUsuario = new PaginaModificacionUsuario();
            App.paginaModificacionUsuario.id = -1;
            ((NavigationPage)this.Parent).PushAsync(App.paginaModificacionUsuario);
        }
    }
}