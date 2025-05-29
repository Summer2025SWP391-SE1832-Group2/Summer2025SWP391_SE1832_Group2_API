using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingOrderController : ControllerBase
    {
        private readonly IShippingOrderService _shippingOrderService;

        public ShippingOrderController(IShippingOrderService shippingOrderService)
        {
            _shippingOrderService = shippingOrderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ShippingOrderDTO>>> GetAllShippingOrders()
        {
            try
            {
                var list = await _shippingOrderService.GetAllShippingOrdersAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShippingOrderDTO>> GetShippingOrderById(int id)
        {
            try
            {
                var item = await _shippingOrderService.GetShippingOrderByIdAsync(id);
                if (item == null)
                    return NotFound($"ShippingOrder with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateShippingOrder([FromBody] ShippingOrderDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _shippingOrderService.CreateShippingOrderAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateShippingOrder([FromBody] ShippingOrderDTO dto)
        {
            try
            {
                var result = await _shippingOrderService.UpdateShippingOrderAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteShippingOrder(int id)
        {
            try
            {
                var result = await _shippingOrderService.DeleteShippingOrderAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
