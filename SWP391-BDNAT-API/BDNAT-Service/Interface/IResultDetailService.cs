using BDNAT_Repository.DTO;
using BDNAT_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IResultDetailService
    {
        Task<List<ResultDetailDTO>> GetAllResultsAsync();
        Task<ResultDetailDTO> GetResultByIdAsync(int id);
        Task<bool> CreateResultAsync(ResultDetailDTO result);
        Task<bool> CreateMultipleResultsAsync(SaveResultDetailRequest dto);
        Task<bool> UpdateResultAsync(ResultDetailDTO result);
        Task<bool> DeleteResultAsync(int id);
        Task<List<ResultDetailDTO>> GetResultDetailsByBookingIdAsync(int BookingId);
        Task<bool> DeleteBySampleIdAsync(int sampleId);
        Task<bool> DeleteByBookingIdAsync(int bookingId);
    }
}
