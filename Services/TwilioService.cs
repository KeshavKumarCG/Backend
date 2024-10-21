using Twilio;
using Twilio.Rest.Api.V2010.Account;

public class TwilioService
{
    public void SendMessage(SmsRequest request)
    {
        var accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
        var authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
        var twilioPhoneNumber = Environment.GetEnvironmentVariable("TWILIO_PHONE_NUMBER");

        TwilioClient.Init(accountSid, authToken);

        var message = MessageResource.Create(
            body: $"Owner: {request.Name}, Phone: {request.Phone}, Email: {request.Email}, Car: {request.CarModel}, Plate: {request.LicensePlate}",
            from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
            to: new Twilio.Types.PhoneNumber("9749494476") // Replace with the valet's phone number
        );
    }
}

// public class SmsRequest
// {
//     public string Name { get; set; }
//     public string Cygid { get; set; }
//     public string Phone { get; set; }
//     public string Email { get; set; }
//     public string CarModel { get; set; }
//     public string LicensePlate { get; set; }
// }
