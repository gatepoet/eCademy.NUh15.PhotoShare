using System;

namespace eCademy.NUh15.PhotoShare.Models
{

    public class UserRating
    {
        public Guid Id { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int Rating { get; set; }
    }
}