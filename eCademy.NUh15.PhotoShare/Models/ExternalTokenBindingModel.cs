using System.ComponentModel.DataAnnotations;

namespace eCademy.NUh15.PhotoShare.Controllers.API
{

    public class ExternalTokenBindingModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        public string Token { get; set; }
    }
}