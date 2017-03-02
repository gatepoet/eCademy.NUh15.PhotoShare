using eCademy.NUh15.PhotoShare.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace eCademy.NUh15.PhotoShare.Services
{
    public class PhotoService
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Photo AddPhoto(Guid id, string filename, byte[] file, string title)
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());

            var photo = new Photo
            {
                Id = id,
                User = user,
                Image = new Models.Image
                {
                    Id = Guid.NewGuid(),
                    Filename = filename,
                    Data = file
                },
                Title = title,
                Timestamp = DateTime.Now
            };

            db.Photos.Add(photo);

            try
            {

            }catch (DbUpdateException ex)
            {
                if (!PhotoExists(photo.Id))
                {
                    throw new DuplicateException<Photo>("Photo already exists.", ex);
                }
                else
                {
                    throw;
                }
            }
            db.SaveChanges();

            return photo;
        }

        public void DeletePhoto(Guid id)
        {
            var photo = db.Photos.Find(id);
            if (photo == null)
            {
                throw new NotFoundException<Photo>(id);
            }
            db.Photos.Remove(photo);
            db.SaveChanges();
        }

        public Photo GetById(Guid id)
        {
            return db.Photos.Find(id);
        }

        public IList<Photo> GetAll()
        {
            return db.Photos
                .OrderByDescending(p => p.Timestamp)
                .ToList();
        }

        public void Save(Photo photo)
        {
            try
            {
                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (PhotoExists(photo.Id))
                {
                    throw new DuplicateException<Photo>("Photo already exists.", ex);
                }
                else
                {
                    throw;
                }
            }

        }

        private bool PhotoExists(Guid id)
        {
            return db.Photos.Count(e => e.Id == id) > 0;
        }

        public double RatePhoto(Guid id, int rating)
        {
            var photo = GetById(id);

            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());

            photo.Rate(rating, user);
            db.SaveChanges();

            return photo.GetScore();
        }
    }
}