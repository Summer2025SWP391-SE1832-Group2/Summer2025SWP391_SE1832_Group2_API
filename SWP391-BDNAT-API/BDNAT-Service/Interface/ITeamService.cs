using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDNAT_Repository.DTO;

namespace BDNAT_Service.Interface
{
    public interface ITeamService
    {   
        Task<List<TeamDTO>> GetAllTeamAsync();
        Task<TeamDTO> GetTeamByIdAsync(int id);
        Task<bool> CreateTeamAsync(TeamDTO Team);
        Task<bool> UpdateTeamAsync(TeamDTO Team);
        Task<bool> DeleteTeamAsync(int id);
    }
}
