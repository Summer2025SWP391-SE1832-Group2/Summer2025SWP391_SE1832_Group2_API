using BDNAT_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Interface
{
    public interface IDoctorTeamRepo : IGenericRepository<DoctorTeam>
    {
        Task<List<DoctorTeam>> GetDoctorTeamsByTeamIdAsync(int teamId);
        Task<List<DoctorTeam>> GetDoctorTeamsByUserIdAsync(int userId);
    }
} 