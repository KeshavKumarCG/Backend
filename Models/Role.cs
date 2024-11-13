using Backend.Models;
using System.ComponentModel.DataAnnotations;

public class Role
{
    [Key]
    public int RoleID { get; set; }  
    public int UserID { get; set; }  
    public int RoleType { get; set; }  
    public User User { get; set; }
}
