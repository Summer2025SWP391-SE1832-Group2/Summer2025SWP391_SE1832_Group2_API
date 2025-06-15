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
        public async Task<ActionResult<List<BookingDisplayDTO>>> GetAllBookingWithSchedule()
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
        public async Task<ActionResult<BookingDisplayDTO>> GetBookingById(int id)
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

        [HttpPost]
        public async Task<ActionResult<bool>> CreateBooking([FromBody] BookingRequestDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _bookingService.CreateBookingAsync(dto);
                return Created("Create Booking Successfully!", result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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

    }
}
