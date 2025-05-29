using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestKitController : ControllerBase
    {
        private readonly ITestKitService _testKitService;

        public TestKitController(ITestKitService testKitService)
        {
            _testKitService = testKitService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TestKitDTO>>> GetAllTestKits()
        {
            try
            {
                var list = await _testKitService.GetAllTestKitsAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestKitDTO>> GetTestKitById(int id)
        {
            try
            {
                var item = await _testKitService.GetTestKitByIdAsync(id);
                if (item == null)
                    return NotFound($"TestKit with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateTestKit([FromBody] TestKitDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _testKitService.CreateTestKitAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateTestKit([FromBody] TestKitDTO dto)
        {
            try
            {
                var result = await _testKitService.UpdateTestKitAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteTestKit(int id)
        {
            try
            {
                var result = await _testKitService.DeleteTestKitAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
