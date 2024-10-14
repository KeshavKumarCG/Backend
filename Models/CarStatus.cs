using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class CarStatus
    {
        [Key]
        public string ID { get; set; }
        public string Status { get; set; }
    }
}
