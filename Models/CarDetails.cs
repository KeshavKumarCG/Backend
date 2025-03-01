using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class CarDetails
    {
        [Key]
        public string ID { get; set; }

        public string CarNumber { get; set; }

        public string CarModel { get; set; }

        // New property for car status
        public string Status { get; set; }
    }
}
