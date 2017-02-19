using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Xamarin.Facebook.Login;
using eCademy.NUh15.PhotoShare.Droid.Services;
using System;
using Android.Util;
using System.Threading.Tasks;

namespace eCademy.NUh15.PhotoShare.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/logo", Theme = "@android:style/Theme.Holo.NoActionBar")]
    public class MainActivity : Activity
    {
        Xamarin.Facebook.ICallbackManager callbackManager;
        FacebookTokenTracker tokenTracker;
        PhotoService photoService;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Facebook.FacebookSdk.SdkInitialize(ApplicationContext);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            Typeface font = Typeface.CreateFromAsset(Assets, "fonts/IndieFlower.ttf");
            FindViewById<TextView>(Resource.Id.logo_text_part1).Typeface = font;
            FindViewById<TextView>(Resource.Id.logo_text_part2).Typeface = font;

            FindViewById<Button>(Resource.Id.main_viewGlobalStream_button)
                .Click += (s, e) => StartActivity(typeof(GlobalStreamActivity));

            callbackManager = Xamarin.Facebook.CallbackManagerFactory.Create();

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
        }

        protected override void OnDestroy()
        {
            tokenTracker.StopTracking();
            base.OnDestroy();
        }

        private void UpdateButtons()
        {
            
        }

        private void SignInWithFacebookToken(LoginResult loginResult)
        {
            var token = loginResult.AccessToken.Token;
            Log.Debug(Application.PackageName, token);
            Task.Run(async () => await photoService.SignInWithFacebookToken(token));
        }
    }
}

