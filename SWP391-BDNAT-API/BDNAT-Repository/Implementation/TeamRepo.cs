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
    public class TeamRepo : GenericRepository<Team>, ITeamRepo
    {
        private static TeamRepo _instance;

        public static TeamRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TeamRepo();
                }
                return _instance;
            }
        }

        public async Task<List<Team>> GetTeamsWithDoctorsAsync()
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Teams
                    .Include(t => t.DoctorTeams)
                        .ThenInclude(dt => dt.User)
                    .ToListAsync();
            }
        }

        public async Task<Team> GetTeamWithDoctorsByIdAsync(int teamId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.Teams
                    .Include(t => t.DoctorTeams)
                        .ThenInclude(dt => dt.User)
                    .FirstOrDefaultAsync(t => t.TeamId == teamId);
            }
        }
    }
} 