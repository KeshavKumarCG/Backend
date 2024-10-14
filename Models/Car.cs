using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Car
    {
        [Key]
        public string ID { get; set; }
        public string CarNumber { get; set; }
        public string CarModel { get; set; }
        public string StatusID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int? OwnerID { get; set; }
        public int? ValetID { get; set; }
    }
}
