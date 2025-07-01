using BDNAT_Repository.DTO;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface ISampleCollectionScheduleService
    {

        Task<List<SampleCollectionScheduleDTO>> GetAllSampleCollectionScheduleAsync();
        Task<List<SampleCollectionScheduleDTO>> GetAllWhereCollectorIsNullAsync();
        Task<SampleCollectionScheduleDTO> GetSampleCollectionScheduleByIdAsync(int id);
        Task<bool> CreateSampleCollectionScheduleAsync(SampleCollectionScheduleDTO SampleCollectionSchedule);
        Task<bool> UpdateSampleCollectionScheduleAsync(SampleCollectionScheduleDTO SampleCollectionSchedule);
        Task<bool> DeleteSampleCollectionScheduleAsync(int id);
        Task<List<UserDTO>> GetAvailableStaffForBooking(int bookingId);
        Task<bool> UpdateScheduleStatusAsync(int id, string status);
        Task<SampleCollectionScheduleDTO> GetScheduleByBookingIdAsync(int bookingId);
        Task<List<SampleCollectionScheduleDTO>> GetSchedulesByCollectorIdAsync(int collectorId);
        Task<bool> UpdateScheduleAssignTaskAsync(int id, int idStaff);
    }
} 

