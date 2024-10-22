using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Backend.Services
{
    public class SmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public SmsService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _fromPhoneNumber = configuration["Twilio:PhoneNumber"];

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
