using eCademy.NUh15.PhotoShare.Models;
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
        public ViewResult Upload()
        {
            return View();
        }

        [Route("Photos/{id}")]
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
                Username = photo.User.UserName
            };
            return View("Details", photoDto);
        }
    }
}