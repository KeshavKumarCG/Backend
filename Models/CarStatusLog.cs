using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
   
public class CarStatusLog
{
    public int ID { get; set; }
    public int UserID { get; set; }
    public string CarID { get; set; }
    public string StatusID { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User User { get; set; }
    public Car Car { get; set; }
    public CarStatus Status { get; set; }
}

}


