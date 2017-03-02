using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace eCademy.NUh15.PhotoShare.Services
{
    
    [Serializable]
    public class DuplicateException<T> : ApplicationException
    {
        public string ResourceReferenceProperty { get; set; }
        public object Id { get; set; }
        public Type Type { get { return typeof(T); } }

        public DuplicateException()
        {
        }

        public DuplicateException(string message)
            : base(message)
        {
        }

        public DuplicateException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DuplicateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            info.AddValue("ResourceReferenceProperty", ResourceReferenceProperty);
            base.GetObjectData(info, context);
        }
    }
}