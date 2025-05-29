using BDNAT_Repository.DTO;
using BDNAT_Repository.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IAuthService
    {
        Task<Token> LoginAsync(LoginRequest request);
        Task<Token> RefreshTokenAsync(string refreshToken);
        Task<string> ResetPassword(string email, string newPass, string? VerifyCode);
        Task<string> RegisterAsync(RegisterRequest request, string? verifyCode);
        Task<string> RegisterStaffAsync(RegisterRequest request, string? verifyCode);
        Task<bool> RevokeTokenAsync(string refreshToken);
        Task<UserDTO> GetUserByEmail(string email);
        public string HashAndTruncatePassword(string password);
    }
}
