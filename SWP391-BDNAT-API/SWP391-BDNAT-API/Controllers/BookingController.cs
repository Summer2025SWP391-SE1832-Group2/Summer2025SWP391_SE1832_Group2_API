using BDNAT_Repository.DTO;
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
        public async Task<ActionResult<List<BookingDTO>>> GetAllBookings()
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
        [HttpGet("/BookingWithSchedule()")]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBookingById(int id)
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

        [HttpPost]
        public async Task<ActionResult<bool>> CreateBooking([FromBody] BookingDTO dto)
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
        public async Task<ActionResult<bool>> UpdateBooking([FromBody] BookingDTO dto)
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

        [HttpPost("receive-hook")]
        public IActionResult ReceiveHook([FromBody] JsonElement data)
        {
            // In dữ liệu webhook ra console
            Console.WriteLine(data.ToString());

            // Trả về 200 OK
            return Ok();
        }
    }
}
