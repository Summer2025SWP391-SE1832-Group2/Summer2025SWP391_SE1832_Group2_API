using BDNAT_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Interface
{
    public interface ITeamServiceRepo : IGenericRepository<TeamService>
    {
        Task<List<TeamService>> GetTeamServicesByTeamIdAsync(int teamId);
        Task<List<TeamService>> GetTeamServicesByServiceIdAsync(int serviceId);
    }
} 