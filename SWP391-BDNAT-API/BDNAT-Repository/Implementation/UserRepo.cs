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
    public class UserRepo : GenericRepository<User>, IUserRepo
    {
        private static UserRepo _instance;

        public static UserRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserRepo();
                }
                return _instance;
            }
        }

        public async Task<User> getUserbyEmail(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<List<User>> GetUsersFilteredAsync(int userId)
        {
            try
            {
                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (currentUser == null)
                    return new List<User>();

                var query = _context.Users.AsNoTracking()
                    .Where(u => u.UserId != userId) // loại chính mình
                    .Where(u => u.Role != "Admin"); // luôn loại admin

                // Nếu không phải admin, loại thêm manager
                if (!string.Equals(currentUser.Role, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(u => u.Role != "Manager");
                }

                return await query.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<User>();
            }
        }

    }
}
