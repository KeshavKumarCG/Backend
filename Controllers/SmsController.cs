using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SmsController : ControllerBase
{
    private readonly TwilioService _twilioService;

    public SmsController(TwilioService twilioService)
    {
        _twilioService = twilioService;
    }

    [HttpPost("send")]
    public IActionResult SendSms([FromBody] SmsRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }

        // Send the SMS using the Twilio service
        _twilioService.SendMessage(request);
        return Ok("Message sent successfully!");
    }
}
