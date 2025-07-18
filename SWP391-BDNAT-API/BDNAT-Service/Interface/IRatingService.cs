using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IRatingService
    {
        Task<List<RatingDTO>> GetAllRatingsAsync();
        Task<RatingDTO> GetRatingByIdAsync(int id);
        Task<List<RatingDTO>> GetRatingByBookIdAsync(int id);
        Task<bool> CreateRatingAsync(RatingDTO rating);
        Task<bool> UpdateRatingAsync(RatingDTO rating);
        Task<bool> DeleteRatingAsync(int id);
    }
}
