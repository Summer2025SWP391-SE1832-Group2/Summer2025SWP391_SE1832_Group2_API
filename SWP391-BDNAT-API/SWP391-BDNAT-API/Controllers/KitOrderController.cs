using BDNAT_Service.DTO;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitOrderController : ControllerBase
    {
        private readonly IKitOrderService _kitOrderService;

        public KitOrderController(IKitOrderService kitOrderService)
        {
            _kitOrderService = kitOrderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<KitOrderDTO>>> GetAllKitOrders()
        {
            try
            {
                var list = await _kitOrderService.GetAllKitOrdersAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KitOrderDTO>> GetKitOrderById(int id)
        {
            try
            {
                var item = await _kitOrderService.GetKitOrderByIdAsync(id);
                if (item == null)
                    return NotFound($"KitOrder with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateKitOrder([FromBody] KitOrderDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _kitOrderService.CreateKitOrderAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateKitOrder([FromBody] KitOrderDTO dto)
        {
            try
            {
                var result = await _kitOrderService.UpdateKitOrderAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteKitOrder(int id)
        {
            try
            {
                var result = await _kitOrderService.DeleteKitOrderAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
