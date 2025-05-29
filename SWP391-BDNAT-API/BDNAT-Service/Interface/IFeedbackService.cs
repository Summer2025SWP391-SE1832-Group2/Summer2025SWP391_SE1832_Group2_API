using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IFeedbackService
    {
        Task<List<FeedbackDTO>> GetAllFeedbacksAsync();
        Task<FeedbackDTO> GetFeedbackByIdAsync(int id);
        Task<bool> CreateFeedbackAsync(FeedbackDTO feedback);
        Task<bool> UpdateFeedbackAsync(FeedbackDTO feedback);
        Task<bool> DeleteFeedbackAsync(int id);
    }
}
