using BDNAT_Service.DTO;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _FeedbackService;

        public FeedbackController(IFeedbackService FeedbackService)
        {
            _FeedbackService = FeedbackService;
        }

        [HttpGet]
        public async Task<ActionResult<List<FeedbackDTO>>> GetAllFeedbacks()
        {
            try
            {
                var Feedbacks = await _FeedbackService.GetAllFeedbacksAsync();
                return Ok(Feedbacks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackDTO>> GetFeedbackById(int id)
        {
            try
            {
                var Feedback = await _FeedbackService.GetFeedbackByIdAsync(id);
                if (Feedback == null)
                    return NotFound($"Feedback with ID {id} not found");
                return Ok(Feedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateFeedback([FromBody] FeedbackDTO FeedbackDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _FeedbackService.CreateFeedbackAsync(FeedbackDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateFeedback([FromBody] FeedbackDTO FeedbackDto)
        {
            try
            {
                var result = await _FeedbackService.UpdateFeedbackAsync(FeedbackDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteFeedback(int id)
        {
            try
            {
                var result = await _FeedbackService.DeleteFeedbackAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}