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
    public partial class PaginaInfo : ContentPage
    {
        public PaginaInfo()
        {
            InitializeComponent();

            this.BackgroundColor = Color.LightBlue;

            var gestoPulsacion = new TapGestureRecognizer();
            gestoPulsacion.Tapped += (s, e) =>
            {
                Device.OpenUri(new Uri(((Label)s).Text));
            };
            lblGithub.GestureRecognizers.Add(gestoPulsacion);
        }
    }
}