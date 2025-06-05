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
    public class ResultDetailService : IResultDetailService
    {
        private readonly IMapper _mapper;

        public ResultDetailService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateResultAsync(ResultDetailDTO result)
        {
            var map = _mapper.Map<ResultDetail>(result);
            return await ResultDetailRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> CreateMultipleResultsAsync(SaveResultDetailRequest dto)
        {
            if (dto.Results == null || !dto.Results.Any())
                return false;

            var resultEntities = dto.Results.Select(r => new ResultDetail
            {
                BookingId = dto.BookingId,
                TestParameterId = r.TestParameterId??0,
                Value = r.Value,
                SampleId = r.SampleId??0
            }).ToList();

            await ResultDetailRepo.Instance.AddRangeAsync(resultEntities);
            return true;
        }

        public async Task<bool> DeleteResultAsync(int id)
        {
            return await ResultDetailRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<ResultDetailDTO>> GetAllResultsAsync()
        {
            var list = await ResultDetailRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<ResultDetailDTO>(x)).ToList();
        }

        public async Task<ResultDetailDTO> GetResultByIdAsync(int id)
        {
            return _mapper.Map<ResultDetailDTO>(await ResultDetailRepo.Instance.GetByIdAsync(id));
        }

        public async Task<List<ResultDetailDTO>> GetResultDetailsByBookingIdAsync(int BookingId)
        {
            var list = await ResultDetailRepo.Instance.GetResultDetailsByBookingIdAsync(BookingId);
            return list.Select(x => _mapper.Map<ResultDetailDTO>(x)).ToList();
        }

        public async Task<bool> UpdateResultAsync(ResultDetailDTO result)
        {
            var map = _mapper.Map<ResultDetail>(result);
            return await ResultDetailRepo.Instance.UpdateAsync(map);
        }
    }

}
