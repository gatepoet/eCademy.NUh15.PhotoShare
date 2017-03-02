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
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using eCademy.NUh15.PhotoShare.Services;

namespace eCademy.NUh15.PhotoShare.Controllers.API
{
    public class PhotosController : ApiController
    {
        private ApplicationUserManager _userManager;
        private PhotoService photoService = new PhotoService();

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
            return photoService.GetAll()
                .Select(p => new PhotoDto {
                    Id = p.Id,
                    Title = p.Title,
                    ImageUrl = Url.Route("Images", new { id = p.Image.Id, thumb = 200 }),
                    PhotoUrl = Url.Route("ViewPhoto", new { id = p.Id }),
                    Username = p.User.UserName,
                    Timestamp = p.Timestamp,
                    Rating = p.GetRating(User.Identity.GetUserId()),
                    Score = p.GetScore()
                });
        }

        // GET: api/Photos/5
        [ResponseType(typeof(PhotoDto))]
        public IHttpActionResult GetPhoto(Guid id, int? thumb)
        {
            var p = photoService.GetById(id);
            if (p == null)
            {
                return NotFound();
            }
            var photo = new PhotoDto
            {
                Id = p.Id,
                Title = p.Title,
                ImageUrl = Url.Route("Images", new { id = p.Image.Id }),
                PhotoUrl = Url.Route("ViewPhoto", new { id = p.Id }),
                Username = p.User.UserName,
                Timestamp = p.Timestamp,
                Rating = p.GetRating(User.Identity.GetUserId()),
                Score = p.GetScore()
            };

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


            try
            {
                photoService.Save(photo);

            }
            catch (DuplicateException<Photo>)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Authorize]
        [Route("api/photos/uploadMobile")]
        [ResponseType(typeof(Photo))]
        public IHttpActionResult PostPhotoMobile(UploadPhotoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = Guid.NewGuid();
            try
            {
                photoService.AddPhoto(
                    id,
                    request.Filename,
                    request.File,
                    request.Title);

                return Ok(id);
            }
            catch (DuplicateException<Photo>)
            {
                return Conflict();
            }
        }


        // POST: api/Photos
        [HttpPost]
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
            var filename = request.Form["filename"];

            var id = Guid.NewGuid();
            try
            {
                var photo = photoService.AddPhoto(
                    id,
                    file.FileName,
                    ConvertToByteArray(file.InputStream),
                    title);
                return CreatedAtRoute("DefaultApi", new { id = photo.Id }, photo);
            }
            catch (DuplicateException<Photo>)
            {
                return Conflict();
            }
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
            try
            {
                photoService.DeletePhoto(id);
            }
            catch (NotFoundException<Photo>)
            {
                return NotFound();
            }

            return Ok();
        }


        [HttpPut]
        [Route("api/photo/{id:guid}/rate/{rating:int}")]
        public IHttpActionResult PutRate(Guid id, int rating)
        {
            var newScore = photoService.RatePhoto(id, rating);

            return Ok(new RateResult(newScore));
        }
    }
}