using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using System.Threading.Tasks;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/valet/notifications")]
    public class ValetNotificationController : ControllerBase
    {
        private readonly CarParkingSystem _context;

        public ValetNotificationController(CarParkingSystem context)
        {
            _context = context;
        }

        // POST route to notify valet
        [HttpPost]
        public async Task<IActionResult> NotifyValet([FromBody] ValetNotificationRequest notification)
        {
            if (notification == null)
            {
                return BadRequest("Invalid notification data.");
            }

            // Check for existing notification with the same phone number and car number
            var existingNotification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.PhoneNumber == notification.PhoneNumber && n.CarNumber == notification.CarNumber);

            if (existingNotification != null)
            {
                // If a duplicate exists, return a conflict response
                return Conflict(new { message = "Notification for this car and phone number already exists." });
            }

            var newNotification = new Notification
            {
                UserName = notification.UserName,
                PhoneNumber = notification.PhoneNumber,
                CarNumber = notification.CarNumber,
                CarModel = notification.CarModel,
                Email = notification.Email,
                NotificationTime = DateTime.UtcNow // Using UTC time for consistency
            };

            _context.Notifications.Add(newNotification);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Valet notified successfully!" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error saving notification: {ex.InnerException?.Message}");
            }
        }

        // Endpoint to get notification count
        [HttpGet("count")]
        public async Task<IActionResult> GetNotificationCount()
        {
            try
            {
                var count = await _context.Notifications.CountAsync();
                return Ok(new { count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching notification count: {ex.Message}");
            }
        }
    }

    // Valet notification request model
    public class ValetNotificationRequest
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string CarNumber { get; set; }
        public string CarModel { get; set; }
        public string Email { get; set; } // Add email field here
    }
}
