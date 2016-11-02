using eCademy.NUh15.PhotoShare.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace eCademy.NUh15.PhotoShare.Controllers
{
    public class ImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(Guid id)
        {
            var file = db.Images.SingleOrDefault(img => img.Id == id);
            if (file == null)
            {
                return HttpNotFound();
            }
            var contentType = GetContentType(file.Filename);
            return File(file.Data, "image/jpeg");
        }

        private string GetContentType(string filename)
        {
            switch (Path.GetExtension(filename))
            {
                case "jpg":
                    return "image/jpeg";
                default:
                    return "";  
            }
        }
    }
}