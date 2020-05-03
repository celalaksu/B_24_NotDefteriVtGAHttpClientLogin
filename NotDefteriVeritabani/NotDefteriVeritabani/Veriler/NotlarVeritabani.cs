#define OFFLINE_SYNC_ENABLED
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using NotDefteriVeritabani.VeriModelleri;
using SQLite;
using Xamarin.Essentials;

namespace NotDefteriVeritabani.Veriler
{
    public class NotlarVeritabani
    {
        //SQLiteAsyncConnection veritabani;

        //readonly IMobileServiceTable<Notlar> notlarTablosu;
        MobileServiceSQLiteStore yerelVeritabani;

        public static IMobileServiceSyncTable<Notlar> notlarTablosu;

        public NotlarVeritabani(string dosyaYolu)
        {

            //veritabani = new SQLiteAsyncConnection(dosyaYolu);
            //veritabani.CreateTableAsync<Notlar>().Wait();
            //notlarTablosu = App.mobilServis.GetTable<Notlar>();
            yerelVeritabani = new MobileServiceSQLiteStore(dosyaYolu);
            yerelVeritabani.DefineTable<Notlar>();

            App.mobilServis.SyncContext.InitializeAsync(yerelVeritabani);


            notlarTablosu = App.mobilServis.GetSyncTable<Notlar>();
        }

        public async void YeniNotEkle(Notlar not)
        {
            //await App.mobilServis.GetTable<Notlar>().InsertAsync(not);
            //await notlarTablosu.InsertAsync(not);
            await notlarTablosu.InsertAsync(not);
            //await App.mobilServis.SyncContext.PushAsync();
            //int sonuc = await veritabani.InsertAsync(not);
            await ZamanUyumsuzSenkronizasyon();            

        }


        public async Task<List<Notlar>> NotlariListele(string eposta)
        {
            //return await notlarTablosu.ReadAsync();
            //return await notlarTablosu.Where().ToListAsync();


            //return await notlarTablosu.ReadAsync();
            //return await App.mobilServis.GetTable<Notlar>().ToListAsync();
            //return await veritabani.Table<Notlar>().ToListAsync();

            //return await notlarTablosu.Select(satir => satir).ToListAsync();
            return await notlarTablosu.Where(p => p.Eposta == eposta).OrderByDescending(p=>p.NotTarih).ToListAsync();
        }


        public async void NotGuncelle(Notlar not)
        {
            //await App.mobilServis.GetTable<Notlar>().UpdateAsync(not);
            //return await veritabani.UpdateAsync(not);
            await notlarTablosu.UpdateAsync(not);
            //await App.mobilServis.SyncContext.PushAsync();
            await ZamanUyumsuzSenkronizasyon();
        }


        public async void NotSil(Notlar not)
        {
            await notlarTablosu.DeleteAsync(not);
            await App.mobilServis.SyncContext.PushAsync();
            //await App.mobilServis.GetTable<Notlar>().DeleteAsync(not);
            //return await veritabani.DeleteAsync(not);            
        }
        /*
       public async Task<Notlar> IdyeGoreNotGetir(int id)
       {

           var not = from u in veritabani.Table<Notlar>()
                     where u.ID == id
                     select u;
           return  await not.FirstOrDefaultAsync();

       }
        */


        public async Task ZamanUyumsuzSenkronizasyon()
        {

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return;
            }
                ReadOnlyCollection<MobileServiceTableOperationError> senkHatalari = null;
                try
                {
                    await App.mobilServis.SyncContext.PushAsync();
                    await notlarTablosu.PullAsync("butunNotlar", "");
                }
                catch (MobileServicePushFailedException hata)
                {
                    if (hata.PushResult != null)
                    {
                        senkHatalari = hata.PushResult.Errors;
                    }
                    await App.Current.MainPage.DisplayAlert("Senkronizasyon hataları oluştu.", hata.Message + " " + hata.PushResult.ToString(), "Tamam");
                }
                catch (Exception hata)
                {
                    await App.Current.MainPage.DisplayAlert("Seknronizasyon hatalar oluştu.", hata.Message, "Tamam");
                }

                if (senkHatalari != null)
                {
                    foreach (var hata in senkHatalari)
                    {
                        if (hata.OperationKind == MobileServiceTableOperationKind.Update && hata.Result != null)
                        {
                            //Güncelleme başarısız oldu, sunucunun kopyasına dönülüyor.
                            await hata.CancelAndUpdateItemAsync(hata.Result);
                        }
                        else
                        {
                            // Yerel değişiklikler iptal edilir
                            await hata.CancelAndDiscardItemAsync();
                        }

                        Debug.WriteLine(@"Senkronizasyonda hata oluştu. Kayıt: {0} ({1}). işlemi iptal edildi.", hata.TableName, hata.Item["id"]);
                    }
                }
            
        }
    }
}
