using BDNAT_Repository.Entities;
using BDNAT_Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Implementation
{
    public class NotificationRepo : GenericRepository<Notification>, INotificationRepo
    {
        private static NotificationRepo _instance;

        public static NotificationRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NotificationRepo();
                }
                return _instance;
            }
        }

        public async Task<List<Notification>> GetNotificationsByUserID(int userId)
        {
            var notis = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.ReceivedAt)
                .ToListAsync();

            return notis;
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null) return false;

            notification.IsRead = true;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
