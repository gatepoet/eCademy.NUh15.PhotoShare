using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using eCademy.NUh15.PhotoShare.Models;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using System.Collections.Generic;

namespace eCademy.NUh15.PhotoShare.Controllers.API
{
    public class PhotosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public PhotosController()
        {
        }

        public PhotosController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: api/Photos
        public IEnumerable<PhotoDto> GetPhotos()
        {
            return db.Photos
                .OrderByDescending(p => p.Timestamp)
                .Take(9)
                .ToList()
                .Select(p => new PhotoDto {
                    Id = p.Id,
                    Title = p.Title,
                    ImageUrl = Url.Route("Images", new { id = p.Image.Id }),
                    PhotoUrl = Url.Route("ViewPhoto", new { id = p.Id }),
                    Username = p.User.UserName,
                    Timestamp = p.Timestamp
                });
        }

        // GET: api/Photos/5
        [ResponseType(typeof(Photo))]
        public IHttpActionResult GetPhoto(Guid id)
        {
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }

        // PUT: api/Photos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPhoto(Guid id, Photo photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != photo.Id)
            {
                return BadRequest();
            }

            db.Entry(photo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Photos
        [Authorize]
        [ResponseType(typeof(Photo))]
        public IHttpActionResult PostPhoto()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = HttpContext.Current.Request;
            var file = request.Files[0];
            var title = request.Form["title"];

            var user = db.Users.Find(User.Identity.GetUserId());

            var photo = new Photo
            {
                Id = Guid.NewGuid(),
                User = user,
                Image = new Image
                {
                    Id = Guid.NewGuid(),
                    Filename = file.FileName,
                    Data = ConvertToByteArray(file.InputStream)
                },
                Title = title,
                Timestamp = DateTime.Now
            };

            db.Photos.Add(photo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PhotoExists(photo.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = photo.Id }, photo);
        }

        private byte[] ConvertToByteArray(Stream inputStream)
        {
            var stream = new MemoryStream();

            inputStream.CopyTo(stream);

            return stream.ToArray();
        }

        // DELETE: api/Photos/5
        public IHttpActionResult DeletePhoto(Guid id)
        {
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return NotFound();
            }

            db.Photos.Remove(photo);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhotoExists(Guid id)
        {
            return db.Photos.Count(e => e.Id == id) > 0;
        }
    }
}