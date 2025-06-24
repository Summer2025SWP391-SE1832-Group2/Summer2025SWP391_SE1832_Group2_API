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
    public class UserWorkScheduleService : IUserWorkScheduleService
    {
        private readonly IMapper _mapper;

        public UserWorkScheduleService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateListUserWorkScheduleAsync(List<UserWorkScheduleDTO> userWorkSchedules)
        {
            var entities = _mapper.Map<List<UserWorkSchedule>>(userWorkSchedules);

            foreach (var schedule in entities)
            {
                var success = await UserWorkScheduleRepo.Instance.InsertAsync(schedule);
                if (!success)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> CreateUserWorkScheduleAsync(UserWorkScheduleDTO userWorkSchedule)
        {
            var map = _mapper.Map<UserWorkSchedule>(userWorkSchedule);
            return await UserWorkScheduleRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteUserWorkScheduleAsync(int id)
        {
            return await UserWorkScheduleRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<UserWorkScheduleDTO>> GetAllUserWorkSchedulesAsync()
        {
            var list = await UserWorkScheduleRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<UserWorkScheduleDTO>(x)).ToList();
        }

        public async Task<UserWorkScheduleDTO> GetUserWorkScheduleByIdAsync(int id)
        {
            var entity = await UserWorkScheduleRepo.Instance.GetByIdAsync(id);
            return _mapper.Map<UserWorkScheduleDTO>(entity);
        }

        public async Task<bool> UpdateUserWorkScheduleAsync(UserWorkScheduleDTO userWorkSchedule)
        {
            var map = _mapper.Map<UserWorkSchedule>(userWorkSchedule);
            return await UserWorkScheduleRepo.Instance.UpdateAsync(map);
        }
    }
}
