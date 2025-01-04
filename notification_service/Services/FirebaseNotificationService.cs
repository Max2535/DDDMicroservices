using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using NotificationService.Models;

namespace NotificationService.Services
{
    public class FirebaseNotificationService
    {
        public FirebaseNotificationService(IConfiguration configuration)
        {
            var firebaseJsonPath = configuration["Firebase:JsonPath"];
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(firebaseJsonPath)
            });
        }

        public async Task SendNotificationAsync(OrderNotification notification)
        {
            var message = new Message
            {
                Token = notification.DeviceToken,
                Notification = new Notification
                {
                    Title = "Order Update",
                    Body = notification.Message
                }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
