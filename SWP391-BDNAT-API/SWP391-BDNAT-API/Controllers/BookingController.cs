using BDNAT_Repository.DTO;
using BDNAT_Repository.Implementation;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookingDisplayDTO>>> GetAllBookings()
        {
            try
            {
                var list = await _bookingService.GetAllBookingsAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("/BookingWithSchedule")]
        public async Task<ActionResult<List<BookingScheduleDTO>>> GetAllBookingWithSchedule()
        {
            try
            {
                var list = await _bookingService.GetAllBookingWithScheduleAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("/checkPending")]
        public async Task<ActionResult<bool>> CheckPending(int userId)
        {
            try
            {
                var hasPending = await _bookingService.CheckPending(userId);
                return Ok(hasPending);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("/BookingWithSample")]
        public async Task<ActionResult<List<BookingSampleDTO>>> GetAllBookingWithSample()
        {
            try
            {
                var list = await _bookingService.GetAllBookingWithSampleAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDisplayDetailDTO>> GetBookingById(int id)
        {
            try
            {
                var item = await _bookingService.GetBookingByIdAsync(id);
                if (item == null)
                    return NotFound($"Booking with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{UserId}/getByUserId")]
        public async Task<ActionResult<List<BookingDisplayDTO>>> GetBookingByUserId(int UserId)
        {
            try
            {
                var item = await _bookingService.GetBookingByUserIdAsync(UserId);
                if (item == null)
                    return NotFound($"Booking with ID {UserId} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("by-collector/{collectorId}")]
        public async Task<IActionResult> GetBookingsByCollector(int collectorId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByCollectorAsync(collectorId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách booking", detail = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateBooking([FromBody] BookingRequestDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        message = "Dữ liệu không hợp lệ",
                        errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });

                var result = await _bookingService.CreateBookingAsync(dto);

                if (string.IsNullOrEmpty(result))
                {
                    return StatusCode(500, new { message = "Không thể tạo đơn hàng. Vui lòng thử lại." });
                }

                return Created("Create Booking Successfully!", result);
            }
            catch (InvalidOperationException ex)
            {
                // Lỗi trùng lịch hoặc logic nghiệp vụ
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Lỗi không xác định
                return StatusCode(500, new { message = "Lỗi hệ thống", detail = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateBooking([FromBody] BookingRequestDTO dto)
        {
            try
            {
                var result = await _bookingService.UpdateBookingAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBooking(int id)
        {
            try
            {
                var result = await _bookingService.DeleteBookingAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("regenerate-qr/{bookingId}")]
        public async Task<IActionResult> RegenerateQr(int bookingId)
        {
            try
            {
                var paymentUrl = await _bookingService.RegeneratePaymentQrAsync(bookingId);
                if (string.IsNullOrEmpty(paymentUrl))
                    return BadRequest("Không thể tạo lại mã thanh toán.");

                return Ok(new { paymentUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

    }
}
