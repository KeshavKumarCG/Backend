using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string CarNumber { get; set; }
        public string CarModel { get; set; }
        public DateTime NotificationTime { get; set; } // Set default value
    }

}
