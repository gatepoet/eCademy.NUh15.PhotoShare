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
using Xamarin.Auth;

namespace eCademy.NUh15.PhotoShare.Droid
{
    public class AndroidSecureDataProvider
    {
        public void Store(string userId, string providerName, IDictionary<string, string> data)
        {
            Clear(providerName);
            var accountStore = AccountStore.Create(Android.App.Application.Context);
            var account = new Account(userId, data);
            accountStore.Save(account, providerName);
        }

        public void Clear(string providerName)
        {
            var accountStore = AccountStore.Create(Android.App.Application.Context);
            var accounts = accountStore.FindAccountsForService(providerName);
            foreach (var account in accounts)
            {
                accountStore.Delete(account, providerName);
            }
        }

        public Dictionary<string, string> Retreive(string providerName)
        {
            var accountStore = AccountStore.Create(Android.App.Application.Context);
            var accounts = accountStore.FindAccountsForService(providerName).FirstOrDefault();
            return (accounts != null) ? accounts.Properties : new Dictionary<string, string>();
        }
    }
}