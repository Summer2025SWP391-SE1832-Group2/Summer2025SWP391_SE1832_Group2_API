using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class UserWorkScheduleService : IUserWorkScheduleService
    {
        public Task<bool> CreateListUserWorkScheduleAsync(List<UserWorkScheduleDTO> UserWorkSchedule)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateUserWorkScheduleAsync(UserWorkScheduleDTO UserWorkSchedule)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserWorkScheduleAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserWorkScheduleDTO>> GetAllUserWorkSchedulesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserWorkScheduleDTO> GetUserWorkScheduleByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserWorkScheduleAsync(UserWorkScheduleDTO UserWorkSchedule)
        {
            throw new NotImplementedException();
        }
    }
}
