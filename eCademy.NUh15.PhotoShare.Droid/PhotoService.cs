using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Android.Graphics;
using System;
using System.Linq;
using Xamarin.Facebook;
using Java.Util;
using Android.Util;
using Android.Widget;

namespace eCademy.NUh15.PhotoShare.Droid
{
    public class PhotoService
    {
        public enum LoginStatus
        {
            LoggedOut,
            NeedsWebApiToken,
            LoggedIn
        }

        private readonly string baseUrl;

        private readonly string GetGlobalStreamPhotosUrl = "/api/photos/";
        private readonly string VerifyExternalTokenUrl = "/api/account/verifyExternalToken";
        public const string UploadPhotoUrl = "/api/photos/uploadMobile";
        private ExternalTokenResponse photoshareToken;
        private AndroidSecureDataProvider secureDataProvider;
        private const string PhotoShareStoreKey = "PhotoShare";

        public LoginStatus GetLoginStatus()
        {
            var facebookToken = AccessToken.CurrentAccessToken;
            if (facebookToken == null || string.IsNullOrWhiteSpace(facebookToken.Token) || facebookToken.Expires.Before(new Date()))
            {
                return LoginStatus.LoggedOut;
            }

            if (photoshareToken == null || string.IsNullOrWhiteSpace(photoshareToken.AccessToken) || photoshareToken.Expires < System.DateTime.Now)
            {
                return LoginStatus.NeedsWebApiToken;
            }

            return LoginStatus.LoggedIn;
        }

        private string AuthorizationHeader
        {
            get
            {
                return "Bearer " + photoshareToken.AccessToken;
            }
        }


        public PhotoService()
        {
#if DEBUG
            this.baseUrl = "http://192.168.1.6:65213/";
#else
            this.baseUrl = "http://photoshare.one/";
#endif
            secureDataProvider = new AndroidSecureDataProvider();
            photoshareToken = secureDataProvider.Retreive(PhotoShareStoreKey)
                .FromDictionary<ExternalTokenResponse>();
        }

        public async Task<Photo[]> GetGlobalStreamPhotos()
        {
            try
            {
                using (var client = CreateWebClient())
                {
                    Log.Info("PhotoShare", "Getting photos");
                    var json = await client.DownloadStringTaskAsync(GetGlobalStreamPhotosUrl);
                    return JsonConvert.DeserializeObject<Photo[]>(json);
                }
            }
            catch (WebException ex)
            {
                Log.Error("PhotoShare", Java.Lang.Throwable.FromException(ex), "Could not get photos");
                return new Photo[0];
            }
        }


        public void SignOut()
        {
            photoshareToken = null;
            secureDataProvider.Clear(PhotoShareStoreKey);
        }

        private WebClient CreateWebClient()
        {
            return new WebClient()
            {
                BaseAddress = baseUrl
            };
        }

        public async Task<Bitmap> GetImage(string url, int size)
        {

            try
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
            catch (WebException ex)
            {
                Log.Error("PhotoShare", Java.Lang.Throwable.FromException(ex), "Could not get image");
                return null;
            }
        }

        public async Task<Guid> UploadPhoto(string title, string filename, byte[] file)
        {
            EnsureLoggedIn();
            try
            {
                using (var client = CreateWebClient())
                {
                    client.Headers.Add(HttpRequestHeader.Authorization, AuthorizationHeader);
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var request = new UploadPhotoRequest { Title = title, Filename = filename, File = file };
                    var result = await client.UploadStringTaskAsync(UploadPhotoUrl, "POST", JsonConvert.SerializeObject(request));
                    var newId = JsonConvert.DeserializeObject<Guid>(result);
                    return newId;
                }
            }
            catch (WebException ex)
            {
                Log.Error("PhotoShare", Java.Lang.Throwable.FromException(ex), "Could not upload photo");
                return Guid.Empty;
            }
        }


        private void EnsureLoggedIn()
        {
            var status = GetLoginStatus();
            if (status == LoginStatus.LoggedIn)
                return;
            throw new NotLoggedInException(status);
        }

        public async Task SignInWithFacebookToken(string token)
        {
            try
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
                        "POST",
                        JsonConvert.SerializeObject(request));

                    var response = JsonConvert.DeserializeObject<ExternalTokenResponse>(result);

                    secureDataProvider.Store(response.UserId, PhotoShareStoreKey, response.ToDictionary());

                    photoshareToken = response;
                }
            }
            catch (Exception ex)
            {
                Log.Error("PhotoShare", Java.Lang.Throwable.FromException(ex), "Could not get photos");
                throw;
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

    public class UploadPhotoRequest
    {
        public string Title
        {
            get;
            set;
        }

        public string Filename
        {
            get;
            set;
        }

        public byte[] File
        {
            get;
            set;
        }
    }
}