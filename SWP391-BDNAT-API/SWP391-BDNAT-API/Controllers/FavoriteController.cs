using BDNAT_Service.DTO;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<FavoriteDTO>>> GetAllFavorites()
        {
            try
            {
                var list = await _favoriteService.GetAllFavoritesAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FavoriteDTO>> GetFavoriteById(int id)
        {
            try
            {
                var item = await _favoriteService.GetFavoriteByIdAsync(id);
                if (item == null)
                    return NotFound($"Favorite with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateFavorite([FromBody] FavoriteDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _favoriteService.CreateFavoriteAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateFavorite([FromBody] FavoriteDTO dto)
        {
            try
            {
                var result = await _favoriteService.UpdateFavoriteAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteFavorite(int id)
        {
            try
            {
                var result = await _favoriteService.DeleteFavoriteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
