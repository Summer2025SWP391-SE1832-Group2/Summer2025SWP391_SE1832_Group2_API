using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsTypeController : ControllerBase
    {
        private readonly IBlogsTypeService _blogsTypeService;

        public BlogsTypeController(IBlogsTypeService blogsTypeService)
        {
            _blogsTypeService = blogsTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BlogsTypeDTO>>> GetAllBlogsTypes()
        {
            try
            {
                var list = await _blogsTypeService.GetAllBlogsTypesAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogsTypeDTO>> GetBlogsTypeById(int id)
        {
            try
            {
                var item = await _blogsTypeService.GetBlogsTypeByIdAsync(id);
                if (item == null)
                    return NotFound($"BlogsType with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateBlogsType([FromBody] BlogsTypeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _blogsTypeService.CreateBlogsTypeAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateBlogsType([FromBody] BlogsTypeDTO dto)
        {
            try
            {
                var result = await _blogsTypeService.UpdateBlogsTypeAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBlogsType(int id)
        {
            try
            {
                var result = await _blogsTypeService.DeleteBlogsTypeAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
