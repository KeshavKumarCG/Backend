using Backend.Models;

public class Car
{
    public string ID { get; set; }
    public string CarNumber { get; set; }
    public string CarModel { get; set; }
    public string StatusID { get; set; }  // Foreign key to CarStatus
    public string CreatedBy { get; set; }
    public int OwnerID { get; set; }  // Foreign key to Users
    public int? ValetID { get; set; }  // Foreign key to Users, nullable for unassigned valet
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public string UpdatedBy { get; set; }

    // Navigation properties for foreign keys
    public virtual CarStatus CarStatus { get; set; }
    public virtual User Owner { get; set; }
    public virtual User Valet { get; set; }
}
