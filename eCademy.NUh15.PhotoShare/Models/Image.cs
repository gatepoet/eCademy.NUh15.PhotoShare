using System;

namespace eCademy.NUh15.PhotoShare.Models
{

    public class Image
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public byte[] Data { get; set; }
    }
}