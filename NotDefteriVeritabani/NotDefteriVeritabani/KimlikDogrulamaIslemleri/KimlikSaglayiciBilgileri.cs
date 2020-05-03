using System;
using System.Collections.Generic;
using System.Text;

namespace NotDefteriVeritabani.KimlikDogrulamaIslemleri
{
    public static class KimlikSaglayiciBilgileri
    {
        public static readonly string IstemciKimligiAndroid = "574981214869-o6icg3jngkcsa0l1g2fh79catr0p3lpn.apps.googleusercontent.com";
        public static readonly string GeriDonusLinkUrlAndroid = "com.googleusercontent.apps.574981214869-o6icg3jngkcsa0l1g2fh79catr0p3lpn:/oauth2redirect";

        public static readonly string IstemciKimligiiOS = "574981214869-3jj2g6kiqm7h9ipecmnt6n7k0adum0q7.apps.googleusercontent.com";
        public static string GeriDonusLinkUrliOS = "com.googleusercontent.apps.574981214869-3jj2g6kiqm7h9ipecmnt6n7k0adum0q7:/oauth2redirect";

        public static readonly string Scope = "email"; //Kullanıcıyı tanımlamak için kullanılacak bilgi
        public static readonly string ClientSecret = "";
        public static readonly string KimlikDogrulamaLinkUrl = "https://accounts.google.com/o/oauth2/auth";
        public static readonly string GecisJetonTokenLinkUrl = "https://www.googleapis.com/oauth2/v4/token";
        public static bool PlatformOzguUIKullanimi = true;
    }
}
