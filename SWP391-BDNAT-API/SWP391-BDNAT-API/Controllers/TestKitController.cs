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

        // GET: api/testkit
        [HttpGet]
        public async Task<ActionResult<List<TestKitDTO>>> GetAllTestKits()
        {
            var list = await _testKitService.GetAllTestKitsAsync();
            return Ok(list);
        }

        // GET: api/testkit/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TestKitDTO>> GetTestKitById(int id)
        {
            var item = await _testKitService.GetTestKitByIdAsync(id);
            if (item == null)
                return NotFound($"TestKit with ID {id} not found");

            return Ok(item);
        }

        // POST: api/testkit
        [HttpPost]
        public async Task<ActionResult<bool>> CreateTestKit([FromBody] TestKitDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _testKitService.CreateTestKitAsync(dto);
            return Ok(result);
        }

        // PUT: api/testkit
        [HttpPut]
        public async Task<ActionResult<bool>> UpdateTestKit([FromBody] TestKitDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _testKitService.UpdateTestKitAsync(dto);
            return result
                ? Ok(true)
                : NotFound($"TestKit with ID {dto.TestKitId} not found");
        }

        // DELETE: api/testkit/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteTestKit(int id)
        {
            var result = await _testKitService.DeleteTestKitAsync(id);
            return result
                ? Ok(true)
                : NotFound($"TestKit with ID {id} not found");
        }
    }

}
