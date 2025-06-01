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
    public class TeamServiceRepo : GenericRepository<TeamService>, ITeamServiceRepo
    {
        private static TeamServiceRepo _instance;

        public static TeamServiceRepo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TeamServiceRepo();
                }
                return _instance;
            }
        }

        public async Task<List<TeamService>> GetTeamServicesByTeamIdAsync(int teamId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.TeamServices
                    .Include(ts => ts.Service)
                    .Where(ts => ts.TeamId == teamId)
                    .ToListAsync();
            }
        }

        public async Task<List<TeamService>> GetTeamServicesByServiceIdAsync(int serviceId)
        {
            using (var context = new DnaTestingDbContext())
            {
                return await context.TeamServices
                    .Include(ts => ts.Team)
                    .Where(ts => ts.ServiceId == serviceId)
                    .ToListAsync();
            }
        }
    }
} 