using System;

namespace eCademy.NUh15.PhotoShare.Models
{
    public class Photo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public virtual Image Image { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime Timestamp { get; set; }
    }
}