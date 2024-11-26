using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Roles
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
