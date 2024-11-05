using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleStatusController : ControllerBase
    {
        private readonly SmsService _smsService;

        public VehicleStatusController(SmsService smsService)
        {
            _smsService = smsService;
        }

        [HttpPost("notify-valet")]
        public IActionResult NotifyValet([FromBody] VehicleStatusNotification notification)
        {
            string message = $"Vehicle Status Update:\n" +
                             $"Owner: {notification.OwnerName}\n" +
                            //   $"Owner Phone: {notification.OwnerPhoneNumber}\n" +
                             $"CYGID: {notification.Cygid}\n" +
                             $"Car Model: {notification.CarModel}\n" +
                             $"License Plate: {notification.LicensePlate}\n" +
                             $"Status: Car is ready for pickup";

            _smsService.SendSms("+919417230210", message);

            return Ok("Valet notified successfully");
        }
    }
}
