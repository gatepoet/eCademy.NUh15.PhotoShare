using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCademy.NUh15.PhotoShare.Controllers
{
    public class PhotosController : Controller
    {
        // GET: Photos
        [Authorize]
        public ActionResult Upload()
        {
            return View();
        }
    }
}