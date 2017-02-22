using Xamarin.Facebook;

namespace eCademy.NUh15.PhotoShare.Droid.Services
{
    class FacebookTokenTracker : AccessTokenTracker
    {
        private PhotoService service;
        public System.Action HandleLoggedIn
        {
            get;
            set;
        }

        public System.Action HandleLoggedOut
        {
            get;
            set;
        }

        public FacebookTokenTracker(PhotoService service)
        {
            this.service = service;
        }

        protected override async void OnCurrentAccessTokenChanged(AccessToken oldAccessToken, AccessToken currentAccessToken)
        {
            if (currentAccessToken == null)
            {
                service.SignOut();
                HandleLoggedOut?.Invoke();
            }
            else
            {
                await service.SignInWithFacebookToken(currentAccessToken.Token);
                HandleLoggedIn?.Invoke();
            }
        }
    }
}
