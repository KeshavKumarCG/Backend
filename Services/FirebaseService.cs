using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class FirebaseService
    {
        public async Task SendNotificationAsync(string token, string title, string body)
        {
            try
            {
                // Prepare the message to send
                var message = new Message
                {
                    Token = token,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = title,
                        Body = body
                    },
                    // Optional: Add custom data if necessary
                    Data = new Dictionary<string, string>
                        {
                            { "key1", "value1" }, // Example custom data
                        }
                };

                // Send the message
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine($"Successfully sent message: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
            }
        }
    }
}
