using Android.Widget;
using Android.Views;
using System.Collections.Generic;
using Android.App;
using System.Linq;
using Android.Util;
using System.Threading.Tasks;
using System;
using Java.Lang;
using System.IO;

namespace eCademy.NUh15.PhotoShare.Droid
{
    public class GlobalStreamAdapter : BaseAdapter<Photo>
    {
        private IList<Photo> photos;

        private int size;

        private Activity activity;
        private readonly PhotoService service;

        public GlobalStreamAdapter(Activity activity, PhotoService service)
        {
            this.activity = activity;
            this.photos = new List<Photo>();
            this.service = service;
            var displayMetrtics = new DisplayMetrics();
            activity.WindowManager.DefaultDisplay.GetMetrics(displayMetrtics);
            size = displayMetrtics.WidthPixels / 3;
        }

        public override Photo this[int position]
        {
            get
            {
                return photos[position];
            }
        }

        public override int Count
        {
            get
            {
                return photos.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return photos[position].Id.GetHashCode();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var photo = this[position];

            ImageView imageView;


            if (convertView == null)
            {
                imageView = new ImageView(activity);
                imageView.LayoutParameters = new GridView.LayoutParams(size, size);
                imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
            }
            else
            {
                imageView = (ImageView)convertView;
            }

            Task.Run(async () => await LoadImage(imageView, photo.ImageUrl));
                
            return imageView;
        }

        public async Task LoadPhotos()
        {
            photos = await service.GetGlobalStreamPhotos();
            activity.RunOnUiThread(() => base.NotifyDataSetInvalidated());
        }

        private async Task LoadImage(ImageView imageView, string imageUrl)
        {
            var image = await service.GetImage(imageUrl, size);
            activity.RunOnUiThread(() => imageView.SetImageBitmap(image));
        }
    }
}