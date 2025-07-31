using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IFirebaseNotificationService
    {
        Task<string> SendNotificationAsync(string title, string body, string deviceToken);
        Task<bool> SaveUserTokenAsync(int userId, string token);
    }
}
