using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserWorkScheduleController : ControllerBase
    {
        private readonly IUserWorkScheduleService _userWorkScheduleService;

        public UserWorkScheduleController(IUserWorkScheduleService userWorkScheduleService)
        {
            _userWorkScheduleService = userWorkScheduleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserWorkScheduleDTO>>> GetAll()
        {
            try
            {
                var list = await _userWorkScheduleService.GetAllUserWorkSchedulesAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getUser_workScheduleByUserID")]
        public async Task<ActionResult<List<UserWorkScheduleDTO>>> GetUser_workScheduleByUserID(int id)
        {
            try
            {
                var list = await _userWorkScheduleService.GetUserWorkSchedulesByUserIDAsync(id);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserWorkScheduleDTO>> GetById(int id)
        {
            try
            {
                var item = await _userWorkScheduleService.GetUserWorkScheduleByIdAsync(id);
                if (item == null)
                    return NotFound($"UserWorkSchedule with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] UserWorkScheduleDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userWorkScheduleService.CreateUserWorkScheduleAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("create-list")]
        public async Task<ActionResult<bool>> CreateList([FromBody] List<UserWorkScheduleDTO> dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userWorkScheduleService.CreateListUserWorkScheduleAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> Update([FromBody] UserWorkScheduleDTO dto)
        {
            try
            {
                var result = await _userWorkScheduleService.UpdateUserWorkScheduleAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                var result = await _userWorkScheduleService.DeleteUserWorkScheduleAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
