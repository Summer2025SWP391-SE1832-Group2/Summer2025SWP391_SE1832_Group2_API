using AutoMapper;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class ParameterService : IParameterService
    {
        private readonly IMapper _mapper;

        public ParameterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateParameterAsync(ParameterDTO parameter)
        {
            var map = _mapper.Map<Parameter>(parameter);
            return await ParameterRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteParameterAsync(int id)
        {
            return await ParameterRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<ParameterDTO>> GetAllParametersAsync()
        {
            var list = await ParameterRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<ParameterDTO>(x)).ToList();
        }

        public async Task<ParameterDTO> GetParameterByIdAsync(int id)
        {
            return _mapper.Map<ParameterDTO>(await ParameterRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateParameterAsync(ParameterDTO parameter)
        {
            var map = _mapper.Map<Parameter>(parameter);
            return await ParameterRepo.Instance.UpdateAsync(map);
        }
    }

}
