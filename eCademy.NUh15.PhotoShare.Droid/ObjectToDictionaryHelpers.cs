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
using Newtonsoft.Json;

namespace eCademy.NUh15.PhotoShare.Droid
{
    public static class ObjectToDictionaryHelper
    {
        public static IDictionary<string, string> ToDictionary(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var json = JsonConvert.SerializeObject(source);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            return dict;
        }

        public static T FromDictionary<T>(this Dictionary<string, string> dict) where T : new()
        {
            var json = JsonConvert.SerializeObject(dict);
            var obj = JsonConvert.DeserializeObject<T>(json);

            return obj;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }
    }
}