using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface ISampleCollectionScheduleService
    {
        Task<List<SampleCollectionScheduleDTO>> GetAllSchedulesAsync();
        Task<SampleCollectionScheduleDTO> GetScheduleByIdAsync(int id);
        Task<SampleCollectionScheduleDTO> GetScheduleByBookingIdAsync(int bookingId);
        Task<List<SampleCollectionScheduleDTO>> GetSchedulesByCollectorIdAsync(int collectorId);
        Task<bool> CreateScheduleAsync(SampleCollectionScheduleDTO schedule);
        Task<bool> UpdateScheduleAsync(SampleCollectionScheduleDTO schedule);
        Task<bool> UpdateScheduleAssignTaskAsync(int id, int idStaff);

        Task<bool> DeleteScheduleAsync(int id);
        Task<bool> UpdateScheduleStatusAsync(int id, string status);
    }
} 