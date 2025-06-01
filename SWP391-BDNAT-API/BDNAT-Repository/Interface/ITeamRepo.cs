using BDNAT_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Interface
{
    public interface ITeamRepo : IGenericRepository<Team>
    {
        Task<List<Team>> GetTeamsWithDoctorsAsync();
        Task<Team> GetTeamWithDoctorsByIdAsync(int teamId);
    }
} 