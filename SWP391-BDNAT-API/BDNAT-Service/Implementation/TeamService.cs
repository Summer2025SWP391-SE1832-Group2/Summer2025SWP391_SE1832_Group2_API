using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BDNAT_Repository.DTO;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Service.Interface;

namespace BDNAT_Service.Implementation
{
    public class TeamServices : ITeamService
    {
        private readonly IMapper _mapper;

        public TeamServices(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateTeamAsync(TeamDTO team)
        {
            var map = _mapper.Map<Team>(team);
            return await TeamRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteTeamAsync(int id)
        {
            return await TeamRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<TeamDTO>> GetAllTeamsAsync()
        {
            var list = await TeamRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<TeamDTO>(x)).ToList();
        }

        public Task<List<TeamDTO>> GetAllTeamAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TeamDTO> GetTeamByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTeamAsync(TeamDTO Team)
        {
            throw new NotImplementedException();
        }
    }
}
