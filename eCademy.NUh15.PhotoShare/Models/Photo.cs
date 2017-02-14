using System;
using System.Collections.Generic;
using System.Linq;

namespace eCademy.NUh15.PhotoShare.Models
{
    public class Photo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public virtual Image Image { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual ICollection<UserRating> Ratings { get; set; }

        public double GetScore()
        {
            return Ratings.Any()
                ? Ratings.Average(rating => rating.Rating)
                : 0;
        }

        public int GetRating(string userId)
        {
            return Ratings.SingleOrDefault(r => r.User.Id.Equals(userId))?.Rating ?? 0;
        }

        public void Rate(int value, ApplicationUser user)
        {
            var rating = Ratings.SingleOrDefault(r => r.User.Equals(user));
            if (rating == null)
            {
                rating = new UserRating
                {
                    Id = Guid.NewGuid(),
                    Photo = this,
                    User = user,
                    Rating = value
                };
                Ratings.Add(rating);
            }
            else
            {
                rating.Rating = value;
            }
        }

    }
}