using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface ISampleService
    {
        Task<List<SampleDTO>> GetAllSamplesAsync();
        Task<SampleDTO> GetSampleByIdAsync(int id);
        Task<List<SampleDTO>> GetSampleByBookingIdAsync(int id);

        Task<bool> CreateSampleAsync(SampleDTO sample);
        Task<bool> UpdateSampleAsync(SampleDTO sample);
        Task<bool> DeleteSampleAsync(int id);
    }
}
