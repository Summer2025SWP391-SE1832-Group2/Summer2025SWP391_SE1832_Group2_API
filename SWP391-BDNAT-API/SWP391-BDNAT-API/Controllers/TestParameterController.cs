using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestParameterController : ControllerBase
    {
        private readonly ITestParameterService _testParameterService;

        public TestParameterController(ITestParameterService testParameterService)
        {
            _testParameterService = testParameterService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TestParameterDTO>>> GetAllTestParameters()
        {
            try
            {
                var list = await _testParameterService.GetAllTestParametersAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestParameterDTO>> GetTestParameterById(int id)
        {
            try
            {
                var item = await _testParameterService.GetTestParameterByIdAsync(id);
                if (item == null)
                    return NotFound($"TestParameter with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateTestParameter([FromBody] TestParameterDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _testParameterService.CreateTestParameterAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateTestParameter([FromBody] TestParameterDTO dto)
        {
            try
            {
                var result = await _testParameterService.UpdateTestParameterAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteTestParameter(int id)
        {
            try
            {
                var result = await _testParameterService.DeleteTestParameterAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
