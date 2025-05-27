using BDNAT_Service.DTO;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
        private readonly IServiceTypeService _serviceTypeService;

        public ServiceTypeController(IServiceTypeService serviceTypeService)
        {
            _serviceTypeService = serviceTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ServiceTypeDTO>>> GetAllServiceTypes()
        {
            try
            {
                var list = await _serviceTypeService.GetAllServiceTypesAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceTypeDTO>> GetServiceTypeById(int id)
        {
            try
            {
                var item = await _serviceTypeService.GetServiceTypeByIdAsync(id);
                if (item == null)
                    return NotFound($"ServiceType with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateServiceType([FromBody] ServiceTypeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _serviceTypeService.CreateServiceTypeAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateServiceType([FromBody] ServiceTypeDTO dto)
        {
            try
            {
                var result = await _serviceTypeService.UpdateServiceTypeAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteServiceType(int id)
        {
            try
            {
                var result = await _serviceTypeService.DeleteServiceTypeAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
