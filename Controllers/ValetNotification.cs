using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("valet")]
    public class ValetController : ControllerBase
    {
        [HttpPost("notifications")]
        public IActionResult NotifyValet([FromBody] ValetNotification notification)
        {
            if (notification == null)
            {
                return BadRequest("Invalid notification data.");
            }

            // Here you can add logic to process the notification data, e.g., save to database, notify valet, etc.

            return Ok(new { message = "Valet notified successfully!" });
        }
    }

    public class ValetNotification
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string CarNumber { get; set; }
        public string CarModel { get; set; }
        // Add any additional properties you need
    }
}
