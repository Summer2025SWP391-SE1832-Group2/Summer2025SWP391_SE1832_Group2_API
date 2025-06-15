using AutoMapper;
using BDNAT_Repository.DTO;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class SampleCollectionScheduleService : ISampleCollectionScheduleService
    {
        private readonly IMapper _mapper;

        public SampleCollectionScheduleService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<SampleCollectionScheduleDTO>> GetAllSampleCollectionScheduleAsync()
        {
            var schedules = await SampleCollectionScheduleRepo.Instance.GetAllAsync();
            return schedules.Select(s => _mapper.Map<SampleCollectionScheduleDTO>(s)).ToList();
        }

        public async Task<SampleCollectionScheduleDTO> GetSampleCollectionScheduleByIdAsync(int id)
        {
            var schedule = await SampleCollectionScheduleRepo.Instance.GetByIdAsync(id);
            return _mapper.Map<SampleCollectionScheduleDTO>(schedule);
        }

        public async Task<SampleCollectionScheduleDTO> GetScheduleByBookingIdAsync(int bookingId)
        {
            var schedule = await SampleCollectionScheduleRepo.Instance.GetScheduleByBookingIdAsync(bookingId);
            return _mapper.Map<SampleCollectionScheduleDTO>(schedule);
        }

        public async Task<List<SampleCollectionScheduleDTO>> GetSchedulesByCollectorIdAsync(int collectorId)
        {
            var schedules = await SampleCollectionScheduleRepo.Instance.GetSchedulesByCollectorIdAsync(collectorId);
            return schedules.Select(s => _mapper.Map<SampleCollectionScheduleDTO>(s)).ToList();
        }

        public async Task<bool> CreateSampleCollectionScheduleAsync(SampleCollectionScheduleDTO schedule)
        {
            var mapSchedule = _mapper.Map<SampleCollectionSchedule>(schedule);
            return await SampleCollectionScheduleRepo.Instance.InsertAsync(mapSchedule);
        }

        public async Task<bool> UpdateSampleCollectionScheduleAsync(SampleCollectionScheduleDTO schedule)
        {
            var mapSchedule = _mapper.Map<SampleCollectionSchedule>(schedule);
            return await SampleCollectionScheduleRepo.Instance.UpdateAsync(mapSchedule);
        }

        public async Task<bool> UpdateScheduleAssignTaskAsync(int id, int idStaff)
        {
            var schedule = await SampleCollectionScheduleRepo.Instance.GetByIdAsync(id);
            if (schedule == null)
                return false;

            schedule.CollectorId = idStaff;
            return await SampleCollectionScheduleRepo.Instance.UpdateAsync(schedule);
        }

        public async Task<bool> DeleteSampleCollectionScheduleAsync(int id)
        {
            return await SampleCollectionScheduleRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<SampleCollectionScheduleDTO>> GetAllWhereCollectorIsNullAsync()
        {
            var list = await SampleCollectionScheduleRepo.Instance.GetAllWhereCollectorIsNullAsync();
            return list.Select(x => _mapper.Map<SampleCollectionScheduleDTO>(x)).ToList();
        }

        public async Task<List<UserDTO>> GetAvailableStaffForSchedule(int scheduleId)
        {
            var schedule = await SampleCollectionScheduleRepo.Instance.GetByIdAsync(scheduleId);
            if (schedule == null || string.IsNullOrEmpty(schedule.Time))
                return new List<UserDTO>();

            // Parse start and end time from format "HH:mm:ss - HH:mm:ss"
            var timeParts = schedule.Time.Split(" - ");
            if (timeParts.Length != 2 ||
                !TimeSpan.TryParse(timeParts[0], out var startTime) ||
                !TimeSpan.TryParse(timeParts[1], out var endTime))
            {
                return new List<UserDTO>();
            }

            var workSchedules = await WorkScheduleRepo.Instance.GetAllAsync();

            // Only consider schedules where both StartTime and EndTime exist
            var validSchedules = workSchedules
                .Where(ws => ws.StartTime.HasValue && ws.EndTime.HasValue)
                .ToList();

            // Find matching work schedule that fully covers the booking time range
            var matchedWorkSchedule = validSchedules.FirstOrDefault(ws =>
                ws.StartTime.Value <= startTime &&
                ws.EndTime.Value >= endTime);

            if (matchedWorkSchedule == null)
                return new List<UserDTO>();

            var userWorkSchedules = await UserWorkScheduleRepo.Instance.GetAllAsync();

            // Get staff with matching work schedule and correct date
            var validUserIds = userWorkSchedules
                .Where(uws =>
                    uws.WorkScheduleId == matchedWorkSchedule.WorkScheduleId &&
                    uws.Date.HasValue &&
                    uws.Date.Value.Date == schedule.CollectionDate.Date &&
                    uws.UserId.HasValue)
                .Select(uws => uws.UserId.Value)
                .ToList();

            var allUsers = await UserRepo.Instance.GetAllAsync();

            var availableStaff = allUsers
                .Where(u => validUserIds.Contains(u.UserId) &&
                            u.Role?.ToLower() == "staff")
                .ToList();

            return _mapper.Map<List<UserDTO>>(availableStaff);
        }

        public async Task<bool> UpdateScheduleStatusAsync(int id, string status)
        {
            var schedule = await SampleCollectionScheduleRepo.Instance.GetByIdAsync(id);
            if (schedule == null)
                return false;

            schedule.Status = status;
            return await SampleCollectionScheduleRepo.Instance.UpdateAsync(schedule);
        }
    }
} 
