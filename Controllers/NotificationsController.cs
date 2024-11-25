using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }

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

        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(NotificationDto notificationDto)
        {
            // Map the incoming DTO to the Notification model
            var notification = new Notification
            {
                UserName = notificationDto.UserName,
                PhoneNumber = notificationDto.PhoneNumber,
                CarNumber = notificationDto.CarNumber,
                CarModel = notificationDto.CarModel,
                Email = notificationDto.Email  // Set the email field
            };

            // Add to the database
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Return the created resource
            return CreatedAtAction(nameof(GetNotification), new { id = notification.NotificationID }, notification);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            // Send an email notification before deleting the record
            var emailSent = await SendEmailAsync(notification);
            if (!emailSent)
            {
                return StatusCode(500, "Failed to send notification email.");
            }

            // Remove the notification from the database
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> SendEmailAsync(Notification notification)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("keshavkumar21167@gmail.com", "bmgfaysjdqkfdcbk"),
                    EnableSsl = true,
                };
              

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("keshavkumar21167@gmail.com"),
                    Subject = "Car Request Completed",
                    Body = $"Dear {notification.UserName},\n\n" +
                           $"Your request for car model {notification.CarModel} ({notification.CarNumber}) has been completed.\n\n" +
                           "Thank you for using our service.",
                    IsBodyHtml = false,
                };

                // Send the email to the user's email address
                mailMessage.To.Add(notification.Email);  // Use the email field

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
