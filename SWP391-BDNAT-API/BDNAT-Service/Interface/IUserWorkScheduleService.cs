using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IUserWorkScheduleService
    {
        Task<List<UserWorkScheduleDTO>> GetAllUserWorkSchedulesAsync();
        Task<UserWorkScheduleDTO> GetUserWorkScheduleByIdAsync(int id);
        Task<bool> CreateUserWorkScheduleAsync(UserWorkScheduleDTO UserWorkSchedule);

        Task<bool> CreateListUserWorkScheduleAsync(List<UserWorkScheduleDTO> UserWorkSchedule);

        Task<bool> UpdateUserWorkScheduleAsync(UserWorkScheduleDTO UserWorkSchedule);
        Task<bool> DeleteUserWorkScheduleAsync(int id);
    }
}
