using Newtonsoft.Json;
using NotDefteriVeritabani.Veriler;
using NotDefteriVeritabani.VeriModelleri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotDefteriVeritabani
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotGirisPage : ContentPage
    {
        
        Notlar not;
        string eposta;       

        public NotGirisPage()
        {
            InitializeComponent();
            not = new Notlar();            
            notGirisStackLayout.BindingContext = not;
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override async void OnAppearing()
        {
            eposta = await SecureStorage.GetAsync("Eposta");
            not.Eposta = eposta;
            
            ListeGuncelle();
            await App.NotlarVeritabani.ZamanUyumsuzSenkronizasyon();
        }

        private async void kaydetButton_Clicked(object sender, EventArgs e)
        {
            not = (Notlar)notGirisStackLayout.BindingContext;            
            not.NotTarih = DateTime.UtcNow;

            //not.Eposta = await SecureStorage.GetAsync("Eposta");
            //not.Eposta = eposta;

            int sonuc=0;

            if (string.IsNullOrEmpty(not.ID))
            {
                App.NotlarVeritabani.YeniNotEkle(not);
                sonuc = 1;
            }
            // sonuc = await App.NotlarVeritabani.YeniNotEkle(not);
            else
            {
                App.NotlarVeritabani.NotGuncelle(not);
                sonuc = 1;
                // sonuc = await App.NotlarVeritabani.NotGuncelle(not);
            }

            if (sonuc ==1)
                await DisplayAlert("İşlem Başarılı", "İşleminiz başarıyla gerçekleştirildi.", "Tamam");
            else
                await DisplayAlert("İşlem Başarısız", "İşleminiz yapılırken hata oluştu.", "Tamam");

            //not.ID = string.Empty;
            //notGirisStackLayout.BindingContext = not; 
            not = new Notlar();
            not.Eposta = eposta;
            notGirisStackLayout.BindingContext = not;
            notGirisi.Text = String.Empty;
            ListeGuncelle();
        }

        
        private  void silButton_Clicked(object sender, EventArgs e)
        {            
            not = (Notlar)notGirisStackLayout.BindingContext;
            
            if (not !=null)
            {
                App.NotlarVeritabani.NotSil(not);
                DisplayAlert("Başarılı", "Notunuz başarıyla silindi.", "Tamam");
            }
            else
            {
                DisplayAlert("Başarısız", "Notunuz silinirken hata oluştu.", "Tamam");
            }

            not = new Notlar();
            not.Eposta = eposta;
            notGirisStackLayout.BindingContext = not;
            notGirisi.Text = String.Empty;
            ListeGuncelle();            
        }

        private void notlarListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            not = (Notlar)e.SelectedItem;
            notGirisStackLayout.BindingContext = not;
        }

        async void ListeGuncelle()
        {
            List<Notlar> notlar = await App.NotlarVeritabani.NotlariListele(eposta);
            notlarListView.ItemsSource = notlar;//.OrderByDescending(t=>t.NotTarih);           
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }        
        
    }
}