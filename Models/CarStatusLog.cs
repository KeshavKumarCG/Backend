using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class CarStatusLog
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string CarID { get; set; }
        public string StatusID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ChangedAt { get; internal set; }
    }
}
