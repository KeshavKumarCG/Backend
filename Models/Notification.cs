public class Notification
{
    public int NotificationID { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string CarNumber { get; set; }
    public string CarModel { get; set; }
    public string Email { get; set; }
    public DateTime NotificationTime { get; set; }

    // New properties for Firebase notifications
    public string Title { get; set; }
    public string Body { get; set; }
}
