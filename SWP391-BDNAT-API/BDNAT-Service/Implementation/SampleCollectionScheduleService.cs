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

        public async Task<List<SampleCollectionScheduleDTO>> GetAllSchedulesAsync()
        {
            var schedules = await SampleCollectionScheduleRepo.Instance.GetAllAsync();
            return schedules.Select(s => _mapper.Map<SampleCollectionScheduleDTO>(s)).ToList();
        }

        public async Task<SampleCollectionScheduleDTO> GetScheduleByIdAsync(int id)
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

        public async Task<bool> CreateScheduleAsync(SampleCollectionScheduleDTO schedule)
        {
            var mapSchedule = _mapper.Map<SampleCollectionSchedule>(schedule);
            return await SampleCollectionScheduleRepo.Instance.InsertAsync(mapSchedule);
        }

        public async Task<bool> UpdateScheduleAsync(SampleCollectionScheduleDTO schedule)
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

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            return await SampleCollectionScheduleRepo.Instance.DeleteAsync(id);
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