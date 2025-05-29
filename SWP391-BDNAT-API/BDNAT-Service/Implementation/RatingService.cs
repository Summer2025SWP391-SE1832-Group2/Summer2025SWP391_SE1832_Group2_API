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
    public class RatingService : IRatingService
    {
        private readonly IMapper _mapper;

        public RatingService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateRatingAsync(RatingDTO rating)
        {
            var map = _mapper.Map<Rating>(rating);
            return await RatingRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteRatingAsync(int id)
        {
            return await RatingRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<RatingDTO>> GetAllRatingsAsync()
        {
            var list = await RatingRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<RatingDTO>(x)).ToList();
        }

        public async Task<RatingDTO> GetRatingByIdAsync(int id)
        {
            return _mapper.Map<RatingDTO>(await RatingRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateRatingAsync(RatingDTO rating)
        {
            var map = _mapper.Map<Rating>(rating);
            return await RatingRepo.Instance.UpdateAsync(map);
        }
    }

}
