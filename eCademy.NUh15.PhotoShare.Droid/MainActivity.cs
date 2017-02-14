using Android.App;
using Android.Widget;
using Android.OS;

namespace eCademy.NUh15.PhotoShare.Droid
{
    [Activity(Label = "eCademy.NUh15.PhotoShare.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);


            FindViewById<Button>(Resource.Id.main_viewGlobalStream_button)
                .Click += (s, e) => StartActivity(typeof(GlobalStreamActivity));
        }
    }
}

