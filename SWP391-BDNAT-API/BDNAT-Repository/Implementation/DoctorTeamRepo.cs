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
    public class DoctorTeamRepo : GenericRepository<DoctorTeam>, IDoctorTeamRepo
    {
        private static DoctorTeamRepo _instance;

        public static DoctorTeamRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DoctorTeamRepo();
                }
                return _instance;
            }
        }

        public async Task<List<DoctorTeam>> GetDoctorTeamsByTeamIdAsync(int teamId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.DoctorTeams
                    .Include(dt => dt.User)
                    .Where(dt => dt.TeamId == teamId)
                    .ToListAsync();
            }
        }

        public async Task<List<DoctorTeam>> GetDoctorTeamsByUserIdAsync(int userId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.DoctorTeams
                    .Include(dt => dt.Team)
                    .Where(dt => dt.UserId == userId)
                    .ToListAsync();
            }
        }
    }
} 