using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class LoginModel
    {
        [Required]
        public string EmailOrPhone { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
