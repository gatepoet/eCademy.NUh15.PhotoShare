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

        private readonly string GetGlobalStreamPhotosUrl = "api/photos/";
        private readonly string VerifyExternalTokenUrl = "api/account/verifyExternalToken";
        private ExternalTokenResponse photoshareToken;
        private AndroidSecureDataProvider secureDataProvider;
        private const string PhotoShareStoreKey = "PhotoShare";

        public PhotoService()
        {
            this.baseUrl = "http://photoshare.one/";
            secureDataProvider = new AndroidSecureDataProvider();
            photoshareToken = secureDataProvider.Retreive(PhotoShareStoreKey)
                .FromDictionary<ExternalTokenResponse>();
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

        public void SignOut()
        {
            photoshareToken = null;
            secureDataProvider.Clear(PhotoShareStoreKey);
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

        public async Task SignInWithFacebookToken(string token)
        {
            using (var client = CreateWebClient())
            {
                var request = new ExternalTokenRequest
                {
                    Provider = "Facebook",
                    Token = token
                };
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");

                var result = await client.UploadStringTaskAsync(
                    VerifyExternalTokenUrl,
                    JsonConvert.SerializeObject(request));

                var response = JsonConvert.DeserializeObject<ExternalTokenResponse>(result);

                //TODO: store in secure data provider

                photoshareToken = response;
            }
        }
    }
    public class ExternalTokenRequest
    {
        public string Token { get; set; }

        public string Provider { get; set; }
    }

    public class ExternalTokenResponse
    {
        [JsonProperty("userName")]
        public string Username { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public double ExpiresIn { get; set; }

        [JsonProperty("issued")]
        public DateTime Issued { get; set; }

        [JsonProperty("expires")]
        public DateTime Expires { get; set; }
    }
}