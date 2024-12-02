using System.ComponentModel.DataAnnotations;

public class Archive
{
    [Key] // Primary key
    public int ID { get; set; }
    
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public string? CarNumber { get; set; }
    public string? CarModel { get; set; }
    public DateTime ArchivedAt { get; set; } // Represents the archive date
}

public class UserArchiveModel
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public string? CarNumber { get; set; }
    public string? CarModel { get; set; }
}

public class UserModel
{
    public string? CYGID { get; set; } 
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int RoleID { get; set; } 
    public string? CreatedBy { get; set; } 
    public string? UpdatedBy { get; set; } 
    public string? CarID { get; set; }
    public string? CarNumber { get; set; }
    public string? CarModel { get; set; }
    public string? StatusID { get; set; } 
}
