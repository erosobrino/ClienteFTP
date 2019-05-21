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
    public partial class PaginaModificacionUsuario : ContentPage
    {
        public int id;
        public PaginaModificacionUsuario()
        {
            InitializeComponent();

            this.BackgroundColor = Color.LightBlue;

            this.Appearing += PaginaModificacionUsuario_Appearing;
        }

        private void PaginaModificacionUsuario_Appearing(object sender, EventArgs e)
        {
            if (id == -1)
                this.Title = "Nuevo usuario";
            else
            {
                this.Title = "Datos del usuario " + id;
                txtNombre.Text = App.paginaUsuarios.usuarioActual.Nombre;
                txtContraseña.Text = App.paginaUsuarios.usuarioActual.Contraseña;
                chkAdmin.IsToggled = App.paginaUsuarios.usuarioActual.EsAdmin;
            }
        }

        private void BtnCancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void BtnAceptar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (txtNombre.Text == "")
                {
                    DisplayAlert("Atención", "Debes introducir el Nombre", "OK");
                    txtNombre.Focus();
                    return;
                }
                if (txtContraseña.Text == "")
                {
                    DisplayAlert("Atención", "Debes introducir la Contraseña", "OK");
                    txtContraseña.Focus();
                    return;
                }
                App.sw.WriteLine("MODIFICAR " + id + " " + txtNombre.Text + " " + txtContraseña.Text + " " + chkAdmin.IsToggled);
                App.sw.Flush();
                string msg = App.sr.ReadLine();
                if (msg == "valido")
                {
                    if (id == -1)
                    {
                        DisplayAlert("Atención", "Usuario añadido", "OK");
                        limpia();
                    }
                    else
                    {
                        DisplayAlert("Atención", "Usuario modificado", "OK");
                        this.Navigation.PopAsync();
                    }
                }
                else
                    DisplayAlert("Atención", "No se ha podido modificar", "OK");
            }
            catch
            {
                App.paginaInicio.errorPerdidaConexion();
            }
        }

        private void TxtNombre_Completed(object sender, EventArgs e)
        {
            txtContraseña.Focus();
            txtContraseña.CursorPosition = txtContraseña.Text.Length;
        }

        private void TxtContraseña_Completed(object sender, EventArgs e)
        {
            chkAdmin.Focus();
        }

        private void limpia()
        {
            txtNombre.Text = "";
            txtContraseña.Text = "";
            chkAdmin.IsToggled = false;
        }
    }
}