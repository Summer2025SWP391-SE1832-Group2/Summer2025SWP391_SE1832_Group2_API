using BDNAT_Repository.DTO;
using BDNAT_Repository.DTO.Auth;
using BDNAT_Service.Implementation;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IFirebaseNotificationService _firebaseService;
        private readonly INotificationService _notificationService;

        public AuthController(IAuthService authService, IFirebaseNotificationService firebaseService, INotificationService notificationService)
        {
            _authService = authService;
            _firebaseService = firebaseService;
            _notificationService = notificationService;
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


        [HttpPost("save-fcm-token")]
        public async Task<IActionResult> SaveToken([FromBody] FCMToken request)
        {
            var result = await _firebaseService.SaveUserTokenAsync(request.UserId, request.Token);
            return Ok(result);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotiTest([FromBody] NotificationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.DeviceToken))
                return BadRequest("Device token is required.");

            try
            {
                var result = await _firebaseService.SendNotificationAsync(
                    request.Title,
                    request.Body,
                    request.DeviceToken
                );

                return Ok(new { MessageId = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("notifications/{userId}")]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            try
            {
                var data = await _notificationService.GetNotificationsByUserId(userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("mark-as-read")]
        public async Task<IActionResult> MarkNotificationAsRead(int notiID)
        {
            var result = await _notificationService.MarkNotificationAsReadAsync(notiID);

            if (!result)
                return NotFound("Notification not found.");

            return Ok("Notification marked as read.");
        }
    }
}
