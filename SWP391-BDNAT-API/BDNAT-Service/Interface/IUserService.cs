using BDNAT_Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<bool> CreateUserAsync(UserDTO User);
        Task<bool> UpdateUserAsync(UserDTO User);
        Task<bool> DeleteUserAsync(int id);
    }
}
