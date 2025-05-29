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
    public class FeedbackService : IFeedbackService
    {
        private readonly IMapper _mapper;

        public FeedbackService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateFeedbackAsync(FeedbackDTO feedback)
        {
            var map = _mapper.Map<Feedback>(feedback);
            return await FeedbackRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteFeedbackAsync(int id)
        {
            return await FeedbackRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<FeedbackDTO>> GetAllFeedbacksAsync()
        {
            var list = await FeedbackRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<FeedbackDTO>(x)).ToList();
        }

        public async Task<FeedbackDTO> GetFeedbackByIdAsync(int id)
        {
            return _mapper.Map<FeedbackDTO>(await FeedbackRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateFeedbackAsync(FeedbackDTO feedback)
        {
            var map = _mapper.Map<Feedback>(feedback);
            return await FeedbackRepo.Instance.UpdateAsync(map);
        }
    }

}
