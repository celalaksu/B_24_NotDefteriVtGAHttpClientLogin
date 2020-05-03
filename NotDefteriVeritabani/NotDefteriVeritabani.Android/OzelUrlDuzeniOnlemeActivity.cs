using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using NotDefteriVeritabani.KimlikDogrulamaIslemleri;

namespace NotDefteriVeritabani.Droid
{
    [Activity(Label = "OzelUrlDuzeniOnlemeActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { "com.googleusercontent.apps.574981214869-o6icg3jngkcsa0l1g2fh79catr0p3lpn" },
        DataPath = "/oauth2redirect")]
    public class OzelUrlDuzeniOnlemeActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            global::Android.Net.Uri uri_android = Intent.Data;
            // Convert Android.Net.Url to Uri
            var uri = new Uri(uri_android.ToString());

            // Close browser 
            var intent = new Intent(this, typeof(MainActivity));
            //intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);

            // Load redirectUrl page
            KimlikDogrulama.oAuth2Authenticator.OnPageLoading(uri);

            this.Finish();
        }
    }
}