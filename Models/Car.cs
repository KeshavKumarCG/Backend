using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
   
public class Car
{
    public string ID { get; set; }
    public string CarNumber { get; set; }
    public string CarModel { get; set; }
    public string StatusID { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? OwnerID { get; set; }
    public int? ValetID { get; set; }

    public CarStatus Status { get; set; }
    public User Owner { get; set; }
    public User Valet { get; set; }
    public ICollection<CarStatusLog> CarStatusLogs { get; set; }
}

}




