using PM2P2T1.Controller;
using PM2P2T1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2P2T1.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        Boolean flag1 = false;
        restAPI countriesApi;
        List<Paises> listPaises;

        public MainPage()
        {
            InitializeComponent();
            countriesApi = new restAPI();
            listPaises = new List<Paises>();
        }

        private async void cmbRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            var region = cmbRegion.SelectedItem as string;

            var internetAccess = Connectivity.NetworkAccess;
            if (internetAccess == NetworkAccess.Internet)
            {
                listPaises = await countriesApi.GetPaises(region);
                ListPaises.ItemsSource = listPaises;

            }
            else
            {
                await DisplayAlert("Error", "Verifica el acceso a internet", "OK");
            }
        }

        private async void ListPaises_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var country = (Paises)e.Item;
            mapView pageDetailCountry = new mapView(country);
            pageDetailCountry.BindingContext = country;
            await Navigation.PushAsync(pageDetailCountry);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
            if (flag1 != true)
            {
                await DisplayAlert("PM2P2T1", "Para comenzar a ver los países, haga click en - Seleccionar una opción -", "OK");
            }

            flag1 = true;
        }
    }
}