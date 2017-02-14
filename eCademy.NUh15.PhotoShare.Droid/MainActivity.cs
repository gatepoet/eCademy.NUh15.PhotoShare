using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;

namespace eCademy.NUh15.PhotoShare.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/logo", Theme = "@android:style/Theme.Holo.NoActionBar")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            Typeface font = Typeface.CreateFromAsset(Assets, "fonts/IndieFlower.ttf");
            FindViewById<TextView>(Resource.Id.logo_text_part1).Typeface = font;
            FindViewById<TextView>(Resource.Id.logo_text_part2).Typeface = font;

            FindViewById<Button>(Resource.Id.main_viewGlobalStream_button)
                .Click += (s, e) => StartActivity(typeof(GlobalStreamActivity));

        }
    }
}

