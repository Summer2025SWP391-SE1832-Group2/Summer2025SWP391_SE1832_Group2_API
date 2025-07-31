using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface INotificationService
    {
        Task<List<BlogDTO>> GetAllNotificationsAsync();

        Task<List<NotificationDTO>> GetNotificationsByNotificationTypeIdAsync(int NotificationTypeId);

        Task<NotificationDTO> GetNotificationByIdAsync(int id);
        Task<bool> CreateNotificationAsync(NotificationDTO Notification);
        Task<bool> UpdateNotificationAsync(NotificationDTO Notification);
        Task<bool> DeleteNotificationAsync(int id);
        Task<List<NotificationDTO>> GetNotificationsByUserId(int userId);
        Task<bool> MarkNotificationAsReadAsync(int notificationId);
    }
}
