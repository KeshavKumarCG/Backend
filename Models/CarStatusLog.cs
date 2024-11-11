using System.ComponentModel.DataAnnotations;

public class CarStatusLog
{
    [Key]
    public int LogID { get; set; }
    public int UserID { get; set; }  // Foreign key to User
    public string CarID { get; set; }  // Foreign key to Car
    public string StatusID { get; set; }  // Foreign key to CarStatus

    // Navigation properties
    public virtual User User { get; set; }
    public virtual Car Car { get; set; }
    public virtual CarStatus CarStatus { get; set; }
}
