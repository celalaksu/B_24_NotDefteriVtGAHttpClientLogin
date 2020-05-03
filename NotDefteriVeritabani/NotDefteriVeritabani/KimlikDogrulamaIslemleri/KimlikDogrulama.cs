using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NotDefteriVeritabani.KimlikDogrulamaIslemleri
{
    public static class KimlikDogrulama
    {
        public static OAuth2Authenticator oAuth2Authenticator;
        

        public static void KimlikDogrula()
        {
            string IstemciKimligi = null;
            string GeriDonusLinkUrl = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    IstemciKimligi = KimlikSaglayiciBilgileri.IstemciKimligiiOS;
                    GeriDonusLinkUrl = KimlikSaglayiciBilgileri.GeriDonusLinkUrliOS;
                    break;

                case Device.Android:
                    IstemciKimligi = KimlikSaglayiciBilgileri.IstemciKimligiAndroid;
                    GeriDonusLinkUrl = KimlikSaglayiciBilgileri.GeriDonusLinkUrlAndroid;
                    break;
            }

            oAuth2Authenticator = new OAuth2Authenticator(
                clientId: IstemciKimligi,
                clientSecret: KimlikSaglayiciBilgileri.ClientSecret,
                scope: KimlikSaglayiciBilgileri.Scope,
                authorizeUrl: new Uri(KimlikSaglayiciBilgileri.KimlikDogrulamaLinkUrl),
                redirectUrl: new Uri(GeriDonusLinkUrl),
                getUsernameAsync: null,
                isUsingNativeUI: KimlikSaglayiciBilgileri.PlatformOzguUIKullanimi,
                accessTokenUrl: new Uri(KimlikSaglayiciBilgileri.GecisJetonTokenLinkUrl))
            {
                AllowCancel = true,
                ShowErrors = false,
                ClearCookiesBeforeLogin = true
            };

            oAuth2Authenticator.Completed += OAuth2Authenticator_Tamamlandi;
            oAuth2Authenticator.Error += OAuth2Authenticator_Hata;


            var kullaniciGirisGorunumu = new OAuthLoginPresenter();
            
            kullaniciGirisGorunumu.Login(oAuth2Authenticator);

        }

        private static async void OAuth2Authenticator_Hata(object sender, AuthenticatorErrorEventArgs e)
        {
            OAuth2Authenticator authenticator = (OAuth2Authenticator)sender;
            if (authenticator != null)
            {
                authenticator.Completed -= OAuth2Authenticator_Tamamlandi;
                authenticator.Error -= OAuth2Authenticator_Hata;
            }

            string uyarıEkranıBaslik = "Kimlik Dogrulama Hatası";
            string uyariMesaji = e.Message;

            Debug.WriteLine($"GOOGLE ile giriş yapılırken hata oluştu! Hata Mesajı: {e.Message}");
            await Application.Current.MainPage.DisplayAlert(uyarıEkranıBaslik, uyariMesaji, "Tamam");
        }

        private static async void OAuth2Authenticator_Tamamlandi(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                try
                {
                    await SecureStorage.SetAsync("GOOGLE", JsonConvert.SerializeObject(e.Account.Properties));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                string email = await GoogleEpostaGetirAsync();

                await SecureStorage.SetAsync("Eposta", email);
                await SecureStorage.SetAsync("Saglayici", "GOOGLE");

                

                await Application.Current.MainPage.Navigation.PushAsync(new NotGirisPage());
            }
            else
            {
                oAuth2Authenticator.OnCancelled();
                oAuth2Authenticator = default(OAuth2Authenticator);
            }
        }

        private static async Task<string> GoogleEpostaGetirAsync()
        {
            string googleJetonKayit = await SecureStorage.GetAsync("GOOGLE");
            string googleJeton = JsonConvert.DeserializeObject<GoogleJeton>(googleJetonKayit).AccessToken;

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"https://www.googleapis.com/oauth2/v1/userinfo?access_token={googleJeton}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Google bilgileri alınamadı. Durum: {httpResponse.StatusCode}");
            }

            string data = await httpResponse.Content.ReadAsStringAsync();

            GoogleVerisi googleData = JsonConvert.DeserializeObject<GoogleVerisi>(data);
            return await Task.FromResult(googleData.Email);
        }

       
      
    }
}
