using AutoMapper;
using BDNAT_Repository.DTO;
using BDNAT_Repository.DTO.Auth;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Service.Interface;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AuthService(IMapper mapper,IEmailService emailService)
        {
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<Token> LoginAsync(LoginRequest request)
        {
            var user = await UserRepo.Instance.getUserbyEmail(request.Username);
            if (user != null)
            {
                // Hash the input password with MD5
                var hashedInputPasswordString = HashAndTruncatePassword(request.Password);

                // Compare the hashed input password with the stored hashed password
                if (hashedInputPasswordString == user.PasswordHash)
                {
                    var token = GenerateToken(user, null);
                    return token;
                }
            }
            return null;
        }

        #region GenerateToken
        /// <summary>
        /// Which will generating token accessible for user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Token GenerateToken(User user, String? RT)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("FullName", user.FullName),
                new Claim("Email", user.Email),
                new Claim("Role", user.Role)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c2VydmVwZXJmZWN0bHljaGVlc2VxdWlja2NvYWNoY29sbGVjdHNsb3Bld2lzZWNhbWU="));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            if (RT != null)
            {
                return new Token()
                {
                    AccessTokenToken = accessToken
                };
            }
            return new Token()
            {
                AccessTokenToken = accessToken
            };
        }
        #endregion

        public Task<Token> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        private string GenerateCode(int length = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();

            string code = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return code;
        }

        public async Task<string> RegisterAsync(RegisterRequest request, string? verifyCode)
        {
            var checkUser = await UserRepo.Instance.getUserbyEmail(request.Email);
            if (checkUser != null)
            {
                return "Your email has been used.";
            }

            if (string.IsNullOrEmpty(verifyCode))
            {
                // Gửi mã xác thực
                var code = GenerateCode();
                VerifyCodeStore.PendingCodes[request.Email] = new VerifyCodeStore.CodeEntry
                {
                    VerifyCode = code,
                    ExpireAt = DateTime.UtcNow.AddMinutes(10)
                };

                await _emailService.SendEmailAsync(request.Email, "Verify your User", code, request.Username);

                return "Verification code has sent to email.";
            }
            else
            {
                // Kiểm tra mã xác thực
                if (!VerifyCodeStore.PendingCodes.ContainsKey(request.Email))
                    throw new Exception("No verification code requested for this email.");

                var entry = VerifyCodeStore.PendingCodes[request.Email];
                if (entry.VerifyCode != verifyCode || entry.ExpireAt < DateTime.UtcNow)
                    throw new Exception("Invalid or expired verification code.");

                // Xác thực xong → xoá khỏi bộ nhớ
                VerifyCodeStore.PendingCodes.Remove(request.Email);

                // Tiếp tục tạo User và User
                var hashedPassword = HashAndTruncatePassword(request.Password);

                // Tạo User trước
                var User = _mapper.Map<User>(request);
                User.PasswordHash = hashedPassword;
                User.Role = "Customer";

                bool UserCreated = await UserRepo.Instance.InsertAsync(User);

                return "Register User Successfully";
            }
        }

        public async Task<string> RegisterStaffAsync(RegisterRequest request, string? verifyCode)
        {
            var checkUser = await UserRepo.Instance.getUserbyEmail(request.Email);
            if (checkUser != null)
            {
                return "Your email has been used.";
            }

            if (string.IsNullOrEmpty(verifyCode))
            {
                // Gửi mã xác thực
                var code = GenerateCode();
                VerifyCodeStore.PendingCodes[request.Email] = new VerifyCodeStore.CodeEntry
                {
                    VerifyCode = code,
                    ExpireAt = DateTime.UtcNow.AddMinutes(10)
                };

                await _emailService.SendEmailAsync(request.Email, "Verify your User", code, request.Username);

                return "Verification code has sent to email.";
            }
            else
            {
                // Kiểm tra mã xác thực
                if (!VerifyCodeStore.PendingCodes.ContainsKey(request.Email))
                    throw new Exception("No verification code requested for this email.");

                var entry = VerifyCodeStore.PendingCodes[request.Email];
                if (entry.VerifyCode != verifyCode || entry.ExpireAt < DateTime.UtcNow)
                    throw new Exception("Invalid or expired verification code.");

                // Xác thực xong → xoá khỏi bộ nhớ
                VerifyCodeStore.PendingCodes.Remove(request.Email);

                // Tiếp tục tạo User và User
                var hashedPassword = HashAndTruncatePassword(request.Password);

                // Tạo User trước
                var User = _mapper.Map<User>(request);
                User.PasswordHash = hashedPassword;
                User.Role = "Staff";

                bool UserCreated = await UserRepo.Instance.InsertAsync(User);
                if (!UserCreated)
                    throw new Exception("Failed to create User");

                return "Register User Successfully";
            }
        }

        public Task<bool> RevokeTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }


        public async Task<UserDTO> GetUserByEmail(string email)
        {
            var User = await UserRepo.Instance.getUserbyEmail(email);
            return _mapper.Map<UserDTO>(User);
        }

        public string HashAndTruncatePassword(string password)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                password = BitConverter.ToString(result).Replace("-", "").ToLowerInvariant();
            }

            // Truncate hash to 16 characters
            password = password.Substring(0, 16);

            return password;
        }

        public async Task<string> ResetPassword(string email, string newPass, string? verifyCode = null)
        {
            var user = await UserRepo.Instance.getUserbyEmail(email);
            if (user == null)
            {
                return "Email không tồn tại.";
            }

            // Nếu chưa có mã xác thực → gửi mã
            if (string.IsNullOrEmpty(verifyCode))
            {
                var code = GenerateCode(); // Tự tạo mã xác thực (6 số chẳng hạn)
                VerifyCodeStore.PendingCodes[email] = new VerifyCodeStore.CodeEntry
                {
                    VerifyCode = code,
                    ExpireAt = DateTime.UtcNow.AddMinutes(10)
                };

                await _emailService.SendEmailAsync(email, "Reset your password", code, user.FullName);
                return "Verification code has been sent to your email.";
            }

            // Nếu có mã xác thực → kiểm tra hợp lệ
            if (!VerifyCodeStore.PendingCodes.ContainsKey(email))
                return "No verification code requested for this email.";

            var entry = VerifyCodeStore.PendingCodes[email];
            if (entry.ExpireAt < DateTime.UtcNow || entry.VerifyCode != verifyCode)
                return "Invalid or expired verification code.";

            // Mã đúng → cho phép đổi mật khẩu
            if (string.IsNullOrEmpty(newPass))
            {
                return "Verification successful. Please provide a new password.";
            }

            // Cập nhật mật khẩu
            user.PasswordHash = HashAndTruncatePassword(newPass);
            var result = await UserRepo.Instance.UpdateAsync(user);

            // Xoá mã khỏi bộ nhớ sau khi dùng
            VerifyCodeStore.PendingCodes.Remove(email);

            return result ? "Password reset successfully." : "Failed to reset password.";
        }
    }

    public static class VerifyCodeStore
    {
        public class CodeEntry
        {
            public string VerifyCode { get; set; }
            public DateTime ExpireAt { get; set; }
        }

        public static Dictionary<string, CodeEntry> PendingCodes = new Dictionary<string, CodeEntry>();
    }
}
