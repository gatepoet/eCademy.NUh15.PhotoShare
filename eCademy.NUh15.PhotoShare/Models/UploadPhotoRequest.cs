using System.ComponentModel.DataAnnotations;

namespace eCademy.NUh15.PhotoShare.Controllers.API
{
    public class UploadPhotoRequest
    {
        [Required]
        public string Title
        {
            get;
            set;
        }
        [Required]
        public string Filename
        {
            get;
            set;
        }

        [Required]
        public byte[] File
        {
            get;
            set;
        }
    }
}