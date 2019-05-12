using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClienteFTP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigUsuario : ContentPage
    {
        public ConfigUsuario()
        {
            InitializeComponent();

            this.BackgroundColor = Color.LightBlue;

            txtPassAntigua.IsPassword = true;
            txtPassNueva.IsPassword = true;
            txtPassNuevaRepetida.IsPassword = true;
        }

        private void BtnCancelar_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
            this.Navigation.PopToRootAsync();
        }

        private void BtnAceptar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (txtPassAntigua.Text == "")
                {
                    DisplayAlert("Atención", "Debes introducir la contraseña antigua", "OK");
                    txtPassAntigua.Focus();
                    return;
                }
                if (txtPassNueva.Text == "")
                {
                    DisplayAlert("Atención", "Debes introducir la nueva contraseña", "OK");
                    txtPassNueva.Focus();
                    return;
                }
                if (txtPassNuevaRepetida.Text == "")
                {
                    DisplayAlert("Atención", "Debes repetir la nueva contraseña", "OK");
                    txtPassAntigua.Focus();
                    return;
                }
                if (txtPassNueva.Text != txtPassNuevaRepetida.Text)
                {
                    DisplayAlert("Atención", "Las contraseñas deben coincidir", "OK");
                    txtPassNueva.Focus();
                    txtPassNueva.CursorPosition = txtPassNueva.Text.Length;
                    return;
                }
                App.sw.WriteLine("USUARIO " + App.nombre + " " + txtPassNueva.Text);
                App.sw.Flush();
                if (App.sr.ReadLine().ToUpper() == "VALIDO")
                {
                    DisplayAlert("Atención", "Se ha modificado correctamente la contraseña", "OK");
                    this.Navigation.PopAsync();
                    this.Navigation.PopToRootAsync();
                }
                else
                    DisplayAlert("Atención", "No se ha podido modificar la contraseña, intentalo de nuevo", "OK");
            }
            catch (Exception ex)
            {
                if (ex is IOException || ex is ObjectDisposedException)
                {
                    App.paginaInicio.errorPerdidaConexion();
                    return;
                }
                throw;
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (App.paginaInfo == null)
                App.paginaInfo = new PaginaInfo();
            ((NavigationPage)this.Parent).PushAsync(App.paginaInfo);
        }

        private void TxtPassAntigua_Completed(object sender, EventArgs e)
        {
            txtPassNueva.Focus();
            txtPassNueva.CursorPosition = txtPassNueva.Text.Length;
        }

        private void TxtPassNueva_Completed(object sender, EventArgs e)
        {
            txtPassNuevaRepetida.Focus();
            txtPassNuevaRepetida.CursorPosition = txtPassNuevaRepetida.Text.Length;
        }

        private void TxtPassNuevaRepetida_Completed(object sender, EventArgs e)
        {
            btnAceptar.Focus();
        }

        private void ChkVerContraseñas_Toggled(object sender, ToggledEventArgs e)
        {
            txtPassAntigua.IsPassword = !chkVerContraseñas.IsToggled;
            txtPassNueva.IsPassword = !chkVerContraseñas.IsToggled;
            txtPassNuevaRepetida.IsPassword = !chkVerContraseñas.IsToggled;
        }
    }
}