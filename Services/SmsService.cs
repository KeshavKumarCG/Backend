// using Twilio;
// using Twilio.Rest.Api.V2010.Account;
// using Twilio.Types;

// namespace Backend.Services
// {
//     public class SmsService
//     {
//         private readonly string _accountSid;
//         private readonly string _authToken;
//         private readonly string _fromPhoneNumber;

//         public SmsService(IConfiguration configuration)
//         {
//             _accountSid = configuration["Twilio:AccountSid"];
//             _authToken = configuration["Twilio:AuthToken"];
//             _fromPhoneNumber = configuration["Twilio:PhoneNumber"];

//             TwilioClient.Init(_accountSid, _authToken);
//         }

//         public void SendSms(string toPhoneNumber, string messageBody)
//         {
//             var message = MessageResource.Create(
//                 body: messageBody,
//                 from: new PhoneNumber("+18186166462"),
//                 to: new PhoneNumber("+918307224389")
//             );
//         }
//     }
// }



using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.Extensions.Options;
using Backend.Models;

namespace Backend.Services
{
    public class SmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public SmsService(IOptions<TwilioSettings> twilioSettings)
        {
            _accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            _authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            _fromPhoneNumber = Environment.GetEnvironmentVariable("TWILIO_FROM_PHONE");

            TwilioClient.Init(_accountSid, _authToken);
        }

        public void SendSms(string toPhoneNumber, string messageBody)
        {
            var message = MessageResource.Create(
                body: messageBody,
                from: new PhoneNumber(_fromPhoneNumber),
                to: new PhoneNumber(toPhoneNumber)
            );
        }
    }
}
