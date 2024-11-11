using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Services;
using System;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly FirebaseService _firebaseService;

        public NotificationsController(ApplicationDbContext context, FirebaseService firebaseService)
        {
            _context = context;
            _firebaseService = firebaseService;
        }

        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(NotificationDto notificationDto)
        {
            // Check for existing notification with the same phone number and car number
            var existingNotification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.PhoneNumber == notificationDto.PhoneNumber && n.CarNumber == notificationDto.CarNumber);

            if (existingNotification != null)
            {
                // If a duplicate exists, return a conflict response
                return Conflict(new { message = "Notification for this car and phone number already exists." });
            }

            var notification = new Notification
            {
                UserName = notificationDto.UserName,
                PhoneNumber = notificationDto.PhoneNumber,
                CarNumber = notificationDto.CarNumber,
                CarModel = notificationDto.CarModel,
                Email = notificationDto.Email,
                NotificationTime = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Send push notification to the specific user (use Firebase token for the user)
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == notificationDto.PhoneNumber);
            if (user != null && !string.IsNullOrEmpty(user.FirebaseToken))
            {
                // Send the push notification to the user via Firebase
                await _firebaseService.SendNotificationAsync(user.FirebaseToken, "New Car Parking Request", "You have a new car parking request!");
            }

            // Return the created notification with status 201
            return CreatedAtAction(nameof(GetNotification), new { id = notification.NotificationID }, notification);
        }

        // Optional: Add this method if you want to get notifications by ID or other means
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);

            if (notification == null)
            {
                return NotFound();
            }

            return notification;
        }
    }
}
