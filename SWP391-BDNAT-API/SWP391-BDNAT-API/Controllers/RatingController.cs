using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RatingDTO>>> GetAllRatings()
        {
            try
            {
                var list = await _ratingService.GetAllRatingsAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RatingDTO>> GetRatingById(int id)
        {
            try
            {
                var item = await _ratingService.GetRatingByIdAsync(id);
                if (item == null)
                    return NotFound($"Rating with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getbyBook/{bookId}")]
        public async Task<ActionResult<RatingDTO>> GetRatingByBookingId(int bookId)
        {
            try
            {
                var item = await _ratingService.GetRatingByBookIdAsync(bookId);
                if (item == null)
                    return NotFound($"Rating with ID {bookId} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateRating([FromBody] RatingDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _ratingService.CreateRatingAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateRating([FromBody] RatingDTO dto)
        {
            try
            {
                var result = await _ratingService.UpdateRatingAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteRating(int id)
        {
            try
            {
                var result = await _ratingService.DeleteRatingAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
