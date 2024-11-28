using System;
using Microsoft.AspNetCore.Mvc;
using Backend.Models; 
using System.Threading.Tasks;
using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/valet")]
    public class ValetNotificationController : ControllerBase
    {
        private readonly CarParkingContext _context;

        public ValetNotificationController(CarParkingContext context)
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
                Email = notification.Email,
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
                return BadRequest($"Error saving notification: {ex.InnerException?.Message}");
            }
        }

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
    public string Email { get; set; }
}
