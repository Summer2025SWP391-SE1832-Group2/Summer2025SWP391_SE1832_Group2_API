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
    }
}
