using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Xamarin.Facebook.Login;
using eCademy.NUh15.PhotoShare.Droid.Services;
using System;
using Android.Util;
using System.Threading.Tasks;
using Xamarin.Facebook.Login.Widget;
using Android.Views;
using Android.Content;
using Xamarin.Facebook;

namespace eCademy.NUh15.PhotoShare.Droid
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity
    {
        ICallbackManager callbackManager;
        FacebookTokenTracker tokenTracker;
        PhotoService photoService;
        private LoginResult loginResult;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            FacebookSdk.SdkInitialize(ApplicationContext);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            Typeface font = Typeface.CreateFromAsset(Assets, "fonts/IndieFlower.ttf");
            FindViewById<TextView>(Resource.Id.logo_text_part1).Typeface = font;
            FindViewById<TextView>(Resource.Id.logo_text_part2).Typeface = font;
            FindViewById<LoginButton>(Resource.Id.login_button).SetReadPermissions("email");
            FindViewById<Button>(Resource.Id.main_viewGlobalStream_button)
                .Click += (s, e) => StartActivity(typeof(GlobalStreamActivity));
            FindViewById<Button>(Resource.Id.main_uploadPhoto_button)
                .Click += (s, e) => StartActivityForResult(typeof(UploadPhotoActivity), RequestCodes.TakePhotoRequest);

            callbackManager = CallbackManagerFactory.Create();

            var loginCallback = new FacebookCallback<LoginResult>
            {
                HandleSuccess = SignInWithFacebookToken,
                HandleCancel = () => Log.Debug(
                    Application.PackageName,
                    "Canceled"),
                HandleError = error => Log.Error(
                    Application.PackageName,
                    Java.Lang.Throwable.FromException(error),
                    "No access")
            };
            LoginManager.Instance.RegisterCallback(callbackManager, loginCallback);

            photoService = new PhotoService();
            tokenTracker = new FacebookTokenTracker(photoService)
            {
                HandleLoggedOut = UpdateButtons
            };

            tokenTracker.StartTracking();

            if (photoService.GetLoginStatus() == PhotoService.LoginStatus.NeedsWebApiToken)
            {
                await photoService.SignInWithFacebookToken(AccessToken.CurrentAccessToken.Token);
            }

            UpdateButtons();
        }

        protected override void OnDestroy()
        {
            tokenTracker.StopTracking();
            base.OnDestroy();
        }

        private void UpdateButtons()
        {
            var uploadPhotoButton = FindViewById<Button>(Resource.Id.main_uploadPhoto_button);
            switch (photoService.GetLoginStatus())
            {
                case PhotoService.LoginStatus.LoggedOut:
                case PhotoService.LoginStatus.NeedsWebApiToken:
                    uploadPhotoButton.Visibility = ViewStates.Gone;
                    break;
                case PhotoService.LoginStatus.LoggedIn:
                    uploadPhotoButton.Visibility = ViewStates.Visible;
                    break;
                default:
                    break;
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }

        private void SignInWithFacebookToken(LoginResult loginResult)
        {
            this.loginResult = loginResult;
            var token = loginResult.AccessToken.Token;
            Log.Debug(Application.PackageName, token);
            Task.Run(async () =>
            {
                await photoService.SignInWithFacebookToken(token);
                RunOnUiThread(() => UpdateButtons());
            });
        }
    }
}

