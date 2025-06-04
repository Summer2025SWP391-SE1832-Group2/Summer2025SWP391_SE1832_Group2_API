using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleCollectionScheduleController : ControllerBase
    {
        private readonly ISampleCollectionScheduleService _service;

        public SampleCollectionScheduleController(ISampleCollectionScheduleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<SampleCollectionScheduleDTO>>> GetAll()
        {
            try
            {
                var list = await _service.GetAllSampleCollectionScheduleAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{scheduleId}/available-staffs")]
        public async Task<IActionResult> GetAvailableStaffs(int scheduleId)
        {
            var staffs = await _service.GetAvailableStaffForSchedule(scheduleId);
            return Ok(staffs);
        }

        [HttpGet("nullCollector")]
        public async Task<ActionResult<List<SampleCollectionScheduleDTO>>> GetAllNullCollector()
        {
            try
            {
                var list = await _service.GetAllWhereCollectorIsNullAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SampleCollectionScheduleDTO>> GetById(int id)
        {
            try
            {
                var item = await _service.GetSampleCollectionScheduleByIdAsync(id);
                if (item == null)
                    return NotFound($"SampleCollectionSchedule with ID {id} not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] SampleCollectionScheduleDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _service.CreateSampleCollectionScheduleAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> Update([FromBody] SampleCollectionScheduleDTO dto)
        {
            try
            {
                var result = await _service.UpdateSampleCollectionScheduleAsync(dto);
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
                var result = await _service.DeleteSampleCollectionScheduleAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
