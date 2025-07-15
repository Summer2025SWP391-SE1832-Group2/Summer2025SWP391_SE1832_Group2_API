using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BlogDTO>>> GetAllBlogs()
        {
            try
            {
                var blogs = await _blogService.GetAllBlogsAsync();
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getfavorite/{uid}")]
        public async Task<ActionResult<List<BlogDTO>>> GetAllFavoriteBlogs(int uid)
        {
            try
            {
                var blogs = await _blogService.GetFavoriteBlogsByUserId(uid);
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDTO>> GetBlogById(int id)
        {
            try
            {
                var blog = await _blogService.GetBlogByIdAsync(id);
                if (blog == null)
                    return NotFound($"Blog with ID {id} not found");
                return Ok(blog);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateBlog([FromBody] BlogDTO blogDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _blogService.CreateBlogAsync(blogDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateBlog([FromBody] BlogDTO blogDto)
        {
            try
            {
                var result = await _blogService.UpdateBlogAsync(blogDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBlog(int id)
        {
            try
            {
                var result = await _blogService.DeleteBlogAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("type/{blogTypeId}")]
        public async Task<ActionResult<List<BlogDTO>>> GetBlogByBlogTypeId(int blogTypeId)
        {
            try
            {
                var blogs = await _blogService.GetBlogsByBlogTypeIdAsync(blogTypeId);
                if (blogs == null || !blogs.Any())
                    return NotFound($"No blogs found for blog type ID {blogTypeId}");
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
