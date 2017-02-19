using System.ComponentModel.DataAnnotations;

namespace eCademy.NUh15.PhotoShare.Controllers.API
{

    public class RegisterExternalTokenBindingModel : ExternalTokenBindingModel
    {
        [Required]
        public string Username { get; set; }
    }
}