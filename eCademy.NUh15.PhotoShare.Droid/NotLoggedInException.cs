using System;
using System.Runtime.Serialization;
using static eCademy.NUh15.PhotoShare.Droid.PhotoService;

namespace eCademy.NUh15.PhotoShare.Droid
{
    [Serializable]
    internal class NotLoggedInException : Exception
    {
        public NotLoggedInException(LoginStatus currentStatus) : base("User is not logged in.")
        {
            CurrentStatus = currentStatus;
            Data.Add("CurrentLoginStatus", currentStatus);
        }

        public LoginStatus CurrentStatus
        {
            get;
            private set;
        }

        public NotLoggedInException()
        {
        }

        public NotLoggedInException(string message) : base(message)
        {
        }

        public NotLoggedInException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotLoggedInException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}