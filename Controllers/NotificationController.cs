using Microsoft.AspNetCore.Mvc;
using Backend.Models; 
using System.Threading.Tasks;
using Backend.Data;
using Microsoft.EntityFrameworkCore;


namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("valet")]
    public class ValetController : ControllerBase
    {
        private readonly CarParkingContext _context;

        public ValetController(CarParkingContext context)
        {
            _context = context;
        }

        [HttpPost("notifications")]
        public async Task<IActionResult> NotifyValet([FromBody] ValetNotification notification)
        {
            if (notification == null)
            {
                return BadRequest("Invalid notification data.");
            }

            var newNotification = new Notification
            {
                UserName = notification.UserName,
                PhoneNumber = notification.PhoneNumber,
                CarNumber = notification.CarNumber,
                CarModel = notification.CarModel,
                NotificationTime = DateTime.Now
            };

            _context.Notifications.Add(newNotification);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Valet notified successfully!" });
            }
            catch (DbUpdateException ex)
            {
                // Handle the database update exception, such as duplicate phone number or car number.
                return BadRequest($"Error saving notification: {ex.InnerException?.Message}");
            }
        }

        // New endpoint to get notification count
        [HttpGet("notifications/count")]
        public async Task<IActionResult> GetNotificationCount()
        {
            var count = await _context.Notifications.CountAsync();
            return Ok(new { count });
        }
    }
}

    public class ValetNotification
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string CarNumber { get; set; }
        public string CarModel { get; set; }
    }