using NotDefteriVeritabani.Veriler;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.WindowsAzure.MobileServices;
using NotDefteriVeritabani.KimlikDogrulamaIslemleri;

namespace NotDefteriVeritabani
{
    public partial class App : Application
    {
       public static MobileServiceClient mobilServis = new MobileServiceClient("https://notdefteri2020.azurewebsites.net");

       

        static string veritabaniAdi = "notlar.db3";
        static string dosyaYolu = FileSystem.AppDataDirectory;
        static readonly string tamDosyaYolu = Path.Combine(dosyaYolu, veritabaniAdi);

        private static NotlarVeritabani notlarVeritabani;
        
        public static NotlarVeritabani NotlarVeritabani
        {
            get 
            { 
                if (notlarVeritabani == null)
                {
                    notlarVeritabani = new NotlarVeritabani(tamDosyaYolu);
                }
                return notlarVeritabani;
            }
            
        }
        

        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new NotGirisPage());
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
