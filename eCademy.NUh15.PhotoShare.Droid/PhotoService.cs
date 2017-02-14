using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Android.Graphics;
using System;
using System.Linq;

namespace eCademy.NUh15.PhotoShare.Droid
{
    public class PhotoService
    {
        private readonly string baseUrl;
        public string BaseUrl => baseUrl;

        private readonly string GetGlobalStreamPhotosUrl = "api/photos/";

        public PhotoService(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public async Task<Photo[]> GetGlobalStreamPhotos()
        {
            using (var client = CreateWebClient())
            {
                try
                {
                    Android.Util.Log.Info("PhotoShare", "Getting photos");
                    var json = await client.DownloadStringTaskAsync(GetGlobalStreamPhotosUrl);
                    return JsonConvert.DeserializeObject<Photo[]>(json);
                }
                catch (WebException ex)
                {
                    Android.Util.Log.Error("PhotoShare", Java.Lang.Throwable.FromException(ex), "Could not get photos");
                    throw;
                }
            }
        }

        private WebClient CreateWebClient()
        {
            return new WebClient() { BaseAddress = baseUrl };
        }

        public async Task<Bitmap> GetImage(string url, int size)
        {
            using (var client = CreateWebClient())
            {
                client.QueryString.Add("thumb", size.ToString());
                var imageBytes = await client.DownloadDataTaskAsync(url);
                if (imageBytes == null || imageBytes.Length == 0)
                {
                    return null;
                }

                var bitmap = await BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length);
                return bitmap;
            }
        }
    }
}