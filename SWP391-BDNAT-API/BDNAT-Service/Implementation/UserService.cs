using AutoMapper;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Service.DTO;
using BDNAT_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;

        public UserService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateUserAsync(UserDTO user)
        {
            var map = _mapper.Map<User>(user);
            return await UserRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await UserRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var list = await UserRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<UserDTO>(x)).ToList();
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            return _mapper.Map<UserDTO>(await UserRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateUserAsync(UserDTO user)
        {
            var map = _mapper.Map<User>(user);
            return await UserRepo.Instance.UpdateAsync(map);
        }
    }

}
