namespace Backend.Models
{
    public class User
    {
        public int ID { get; set; }
        public string CYGID { get; set; }
        public string Name { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Role { get; set; } 
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? CarID { get; set; }
    }
}
