using AutoMapper;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class WorkScheduleService : IWorkScheduleService
    {
        private readonly IMapper _mapper;

        public WorkScheduleService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateListWorkScheduleAsync(List<WorkScheduleDTO> schedules)
        {
            var scheduleEntities = _mapper.Map<List<WorkSchedule>>(schedules);

            foreach (var schedule in scheduleEntities)
            {
                var success = await WorkScheduleRepo.Instance.InsertAsync(schedule);
                if (!success)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> CreateWorkScheduleAsync(WorkScheduleDTO schedule)
        {
            var map = _mapper.Map<WorkSchedule>(schedule);
            return await WorkScheduleRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteWorkScheduleAsync(int id)
        {
            return await WorkScheduleRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<WorkScheduleDTO>> GetAllWorkSchedulesAsync()
        {
            var list = await WorkScheduleRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<WorkScheduleDTO>(x)).ToList();
        }

        public async Task<WorkScheduleDTO> GetWorkScheduleByIdAsync(int id)
        {
            var entity = await WorkScheduleRepo.Instance.GetByIdAsync(id);
            return _mapper.Map<WorkScheduleDTO>(entity);
        }

        public async Task<bool> UpdateWorkScheduleAsync(WorkScheduleDTO schedule)
        {
            var map = _mapper.Map<WorkSchedule>(schedule);
            return await WorkScheduleRepo.Instance.UpdateAsync(map);
        }
    }
}
