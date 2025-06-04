using AutoMapper;
using BDNAT_Repository.DTO;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Service.Interface;
using System.Collections.Generic;
using System.Linq;
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
            var list = await SampleCollectionScheduleRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<SampleCollectionScheduleDTO>(x)).ToList();
        }

        public async Task<SampleCollectionScheduleDTO> GetSampleCollectionScheduleByIdAsync(int id)
        {
            var entity = await SampleCollectionScheduleRepo.Instance.GetByIdAsync(id);
            return _mapper.Map<SampleCollectionScheduleDTO>(entity);
        }

        public async Task<bool> CreateSampleCollectionScheduleAsync(SampleCollectionScheduleDTO schedule)
        {
            var entity = _mapper.Map<SampleCollectionSchedule>(schedule);
            return await SampleCollectionScheduleRepo.Instance.InsertAsync(entity);
        }

        public async Task<bool> UpdateSampleCollectionScheduleAsync(SampleCollectionScheduleDTO schedule)
        {
            var entity = _mapper.Map<SampleCollectionSchedule>(schedule);
            return await SampleCollectionScheduleRepo.Instance.UpdateAsync(entity);
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

            if (!TimeSpan.TryParse(schedule.Time, out var collectionTime))
                return new List<UserDTO>();

            var workSchedules = await WorkScheduleRepo.Instance.GetAllAsync();
            var matchedWorkSchedule = workSchedules.FirstOrDefault(ws =>
                ws.StartTime.HasValue && ws.EndTime.HasValue &&
                collectionTime >= ws.StartTime.Value &&
                collectionTime <= ws.EndTime.Value);

            if (matchedWorkSchedule == null)
                return new List<UserDTO>();

            var userWorkSchedules = await UserWorkScheduleRepo.Instance.GetAllAsync();
            var validUserIds = userWorkSchedules
                .Where(u => u.WorkScheduleId == matchedWorkSchedule.WorkScheduleId &&
                            u.Date.HasValue &&
                            u.Date.Value.Date == schedule.CollectionDate.Date &&
                            u.UserId.HasValue)
                .Select(u => u.UserId.Value)
                .ToList();

            var allUsers = await UserRepo.Instance.GetAllAsync();
            var availableStaff = allUsers
                .Where(u => validUserIds.Contains(u.UserId) &&
                            u.Role?.ToLower() == "staff")
                .ToList();

            var mapped = _mapper.Map<List<UserDTO>>(availableStaff);
            return mapped;
        }
    }
}
