using BDNAT_Repository.DTO;
using BDNAT_Repository.DTO.Auth;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.LoginAsync(request);
                if (token == null)
                {
                    return Unauthorized(new APIResponse(false, "Invalid username or password", null));
                }
                var tokenData = new
                {
                    token = token.AccessTokenToken,
                    refreshToken = token.RefreshToken,
                    expiredAt = token.ExpiredAt
                };
                return Ok(new APIResponse(true, "Login successful", tokenData));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(false, $"Internal server error: {ex.Message}", null));
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<APIResponse>> RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                var token = await _authService.RefreshTokenAsync(refreshToken);
                return Ok(APIResponse.Ok(token, "Token refreshed successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.Error($"Internal server error: {ex.Message}"));
            }
        }

        [HttpPut("reset-password")]
        public async Task<ActionResult<bool>> ResetPassword(string email, string newPass, string? VerifyCode)
        {
            try
            {
                var result = await _authService.ResetPassword(email, newPass, VerifyCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<APIResponse>> Register([FromBody] RegisterRequest request, string? VerifyCode)
        {
            try
            {
                var result = await _authService.RegisterAsync(request, VerifyCode);
                return Ok(APIResponse.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.Error($"Internal server error: {ex.Message}"));
            }
        }

        [HttpPost("register-staff")]
        public async Task<ActionResult<APIResponse>> RegisterStaff([FromBody] RegisterRequest request, string? VerifyCode)
        {
            try
            {
                var result = await _authService.RegisterStaffAsync(request, VerifyCode);
                return Ok(APIResponse.Ok(result, "Register Successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.Error($"Internal server error: {ex.Message}"));
            }
        }
    }
}
