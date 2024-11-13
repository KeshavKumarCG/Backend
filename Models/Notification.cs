// using System.ComponentModel.DataAnnotations;

// namespace Backend.Models
// {
//     public class Notification
//     {
//         public int NotificationID { get; set; }
//         public string UserName { get; set; }
//         public string PhoneNumber { get; set; }
//         public string CarNumber { get; set; }
//         public string CarModel { get; set; }
//         public DateTime NotificationTime { get; set; } // Set default value
//     }

// }


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        
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

        public string Email { get; set; }

        public DateTime NotificationTime { get; set; } = DateTime.Now;
    }
}
