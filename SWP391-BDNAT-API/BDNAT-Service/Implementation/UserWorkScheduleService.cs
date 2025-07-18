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

        public async Task<List<UserWorkScheduleDTO>> GetUserWorkSchedulesByUserIDAsync(int userId)
        {
            var userWorkSchedules = await UserWorkScheduleRepo.Instance.GetUserWorkSchedulesByUserIdAsync(userId);

            var result = userWorkSchedules.Select(uws => new UserWorkScheduleDTO
            {
                UserWorkScheduleId = uws.UserWorkScheduleId,
                UserId = uws.UserId,
                WorkScheduleId = uws.WorkScheduleId,
                Title = uws.WorkSchedule?.Title,
                Date = uws.Date
            }).ToList();

            return result;
        }

        public async Task<List<UserDTO>> GetUsersByWorkScheduleIdAsync(int workScheduleId, DateTime? date)
        {
            var users = await UserWorkScheduleRepo.Instance.GetUsersAssignedToScheduleAsync(workScheduleId, date);

            return users.Select(u => _mapper.Map<UserDTO>(u)).ToList();
        }


    }
}
