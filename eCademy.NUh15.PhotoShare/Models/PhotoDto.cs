using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCademy.NUh15.PhotoShare.Models
{
    public class PhotoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Timestamp { get; set; }
    }
}