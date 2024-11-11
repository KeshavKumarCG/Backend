public class Role
{
    public int ID { get; set; }
    public int UserID { get; set; }  // Foreign key to Users

    public virtual User User { get; set; }
}
