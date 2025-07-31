using BDNAT_Repository.Implementation;
using BDNAT_Service.Interface;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        public FirebaseNotificationService()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "swp391-dna-testing-system-firebase-adminsdk-fbsvc-71317f55e1.json");

                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(filePath)
                });
            }
        }

        public async Task<string> SendNotificationAsync(string title, string body, string deviceToken)
        {
            var message = new Message()
            {
                Token = deviceToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body,
                }
            };

            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        public async Task<bool> SaveUserTokenAsync(int userId, string token)
        {
            var result = await FirebaseNotificationRepo.Instance.SaveUserTokenAsync(userId, token);
            return result;
        }
    }
}
