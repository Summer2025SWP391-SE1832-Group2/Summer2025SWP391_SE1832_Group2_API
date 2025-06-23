using BDNAT_Repository.DTO;
using BDNAT_Repository.Entities;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultDetailController : ControllerBase
    {
        private readonly IResultDetailService _resultService;
        private readonly ITestParameterService _testParameterService;

        public ResultDetailController(IResultDetailService resultService, ITestParameterService testParameterService)
        {
            _resultService = resultService;
            _testParameterService = testParameterService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ResultDetailDTO>>> GetAllResults()
        {
            try
            {
                var list = await _resultService.GetAllResultsAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{bookingId}/getAllResultByBookingId")]
        public async Task<ActionResult<List<ResultDetailDTO>>> GetAllResultsByBookingID(int bookingId)
        {
            try
            {
                var list = await _testParameterService.GetResultWithParameterInfoAsync(bookingId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResultDetailDTO>> GetResultById(int id)
        {
            try
            {
                var item = await _resultService.GetResultByIdAsync(id);
                if (item == null)
                    return NotFound($"Result with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("createMultipleResults")]
        public async Task<IActionResult> CreateMultipleResults([FromBody] SaveResultDetailRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _resultService.CreateMultipleResultsAsync(dto);
            if (success)
                return Ok("Cập nhật kết quả thành công.");

            return BadRequest("Cập nhật thất bại. Vui lòng kiểm tra lại dữ liệu.");
        }

        [HttpPut("updateMultipleResults")]
        public async Task<IActionResult> UpdateMultipleResults([FromBody] SaveResultDetailRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _resultService.UpdateMultipleResultsAsync(dto);

                if (success)
                    return Ok(new { message = "Cập nhật kết quả thành công." });

                return BadRequest(new { message = "Cập nhật thất bại. Vui lòng kiểm tra lại dữ liệu." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống", detail = ex.Message });
            }
        }

        [HttpPost("createResult")]
        public async Task<ActionResult<bool>> CreateResult([FromBody] ResultDetailDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _resultService.CreateResultAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateResult([FromBody] ResultDetailDTO dto)
        {
            try
            {
                var result = await _resultService.UpdateResultAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteResult(int id)
        {
            try
            {
                var result = await _resultService.DeleteResultAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("sample/{sampleId}")]
        public async Task<IActionResult> DeleteBySampleId(int sampleId)
        {
            var success = await _resultService.DeleteBySampleIdAsync(sampleId);
            if (success)
                return Ok("Deleted results by SampleId successfully.");
            return NotFound("No results found for the given SampleId.");
        }

        // DELETE BY BOOKING ID
        [HttpDelete("booking/{bookingId}")]
        public async Task<IActionResult> DeleteByBookingId(int bookingId)
        {
            var success = await _resultService.DeleteByBookingIdAsync(bookingId);
            if (success)
                return Ok("Deleted results by BookingId successfully.");
            return NotFound("No results found for the given BookingId.");
        }
    }
}
