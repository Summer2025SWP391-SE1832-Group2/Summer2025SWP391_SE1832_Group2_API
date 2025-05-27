using AutoMapper;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Service.DTO;
using BDNAT_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class ResultService : IResultService
    {
        private readonly IMapper _mapper;

        public ResultService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateResultAsync(ResultDTO result)
        {
            var map = _mapper.Map<Result>(result);
            return await ResultRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteResultAsync(int id)
        {
            return await ResultRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<ResultDTO>> GetAllResultsAsync()
        {
            var list = await ResultRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<ResultDTO>(x)).ToList();
        }

        public async Task<ResultDTO> GetResultByIdAsync(int id)
        {
            return _mapper.Map<ResultDTO>(await ResultRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateResultAsync(ResultDTO result)
        {
            var map = _mapper.Map<Result>(result);
            return await ResultRepo.Instance.UpdateAsync(map);
        }
    }

}
