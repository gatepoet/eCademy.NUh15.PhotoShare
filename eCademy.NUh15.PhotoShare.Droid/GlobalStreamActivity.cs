using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Android.Util;
using Java.Lang;
using System.IO;

namespace eCademy.NUh15.PhotoShare.Droid
{
    [Activity(Label = "GlobalStreamActivity")]
    public class GlobalStreamActivity : Activity
    {
        private GlobalStreamAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.GlobalStream);

            var service = new PhotoService();
            adapter = new GlobalStreamAdapter(this, service);
            var grid = FindViewById<GridView>(Resource.Id.globalstream_photos);
            grid.Adapter = adapter;

            Task.Run(async () => await adapter.LoadPhotos());
        }
    }
}