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
    public class FirebaseNotificationRepo : GenericRepository<UserNotificationToken>, IFirebaseNotificationRepo
    {
        private static FirebaseNotificationRepo _instance;

        public static FirebaseNotificationRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FirebaseNotificationRepo();
                }
                return _instance;
            }
        }

        public async Task<bool> SaveUserTokenAsync(int userId, string token)
        {
            // Xóa tất cả token cũ của user
            var existingTokens = await _context.UserNotificationTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            _context.UserNotificationTokens.RemoveRange(existingTokens);

            // Thêm token mới
            var newToken = new UserNotificationToken
            {
                UserId = userId,
                Token = token,
                LastUpdated = DateTime.UtcNow,
                IsRevoked = false
            };

            await _context.UserNotificationTokens.AddAsync(newToken);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<UserNotificationToken?> GetLatestValidTokenByUserIdAsync(int userId)
        {
            return await _context.UserNotificationTokens
                .Where(t => t.UserId == userId && t.IsRevoked == false && !string.IsNullOrEmpty(t.Token))
                .OrderByDescending(t => t.LastUpdated)
                .FirstOrDefaultAsync();
        }
    }
}
