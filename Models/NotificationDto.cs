using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class NotificationDto
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }
        
        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string CarNumber { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string CarModel { get; set; }
    }
}
