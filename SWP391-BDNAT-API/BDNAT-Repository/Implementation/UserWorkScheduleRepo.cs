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
    public class UserWorkScheduleRepo : GenericRepository<UserWorkSchedule>, IUserWorkScheduleRepo
    {
        private static UserWorkScheduleRepo _instance;

        public static UserWorkScheduleRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserWorkScheduleRepo();
                }
                return _instance;
            }
        }

        public async Task<List<UserWorkSchedule>> GetUserWorkSchedulesByUserIdAsync(int userId)
        {
            using var context = new DnaTestingDbContext();
            return await context.UserWorkSchedules
                .Include(uws => uws.WorkSchedule)
                .Where(uws => uws.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<User>> GetUsersAssignedToScheduleAsync(int workScheduleId, DateTime? date)
        {
            var query = _context.UserWorkSchedules
                .AsNoTracking()
                .Include(uws => uws.User)
                .Where(uws => uws.WorkScheduleId == workScheduleId);

            if (date.HasValue)
            {
                query = query.Where(uws => uws.Date != null && uws.Date.Value.Date == date.Value.Date);
            }

            return await query
                .Select(uws => uws.User!)
                .ToListAsync();
        }


    }
}
