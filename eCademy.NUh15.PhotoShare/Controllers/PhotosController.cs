using eCademy.NUh15.PhotoShare.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCademy.NUh15.PhotoShare.Controllers
{
    public class PhotosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        [Route("Photos/Upload")]
        public ViewResult Upload()
        {
            return View();
        }

        [Route("Photos/{id}", Name = "ViewPhoto")]
        public ActionResult Index(Guid id)
        {
            var photo = db.Photos.SingleOrDefault(p => p.Id == id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            var photoDto = new PhotoDto
            {
                Id = photo.Id,
                Title = photo.Title,
                ImageUrl = Url.RouteUrl("Images", new { id = photo.Image.Id }),
                PhotoUrl = Url.RouteUrl("ViewPhoto", new { id = photo.Id }),
                Username = photo.User.UserName,
                Timestamp = photo.Timestamp,
                Rating = photo.GetRating(User.Identity.GetUserId()),
                Score = photo.GetScore()
            };
            return View("Details", photoDto);
        }
    }
}