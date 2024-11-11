using Backend.Models;

public class User
{
    public int ID { get; set; }
    public string CYGID { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public string FirebaseToken { get; set; } 

    public virtual Role Role { get; set; }
}
