using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotDefteriVeritabani.KimlikDogrulamaIslemleri
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        

        

        public LoginPage()
        {
            InitializeComponent();
            
           
        }

        private void girisButton_Clicked(object sender, EventArgs e)
        {
            KimlikDogrulama.KimlikDogrula();
            
            
        }

       
       
}
}