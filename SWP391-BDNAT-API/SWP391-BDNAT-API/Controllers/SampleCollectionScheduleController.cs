﻿using BDNAT_Repository.DTO;
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
        private readonly ISampleCollectionScheduleService _scheduleService;

        public SampleCollectionScheduleController(ISampleCollectionScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SampleCollectionScheduleDTO>>> GetAll()
        {
            try
            {
                var list = await _scheduleService.GetAllSampleCollectionScheduleAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{bookingId}/available-staffs")]
        public async Task<IActionResult> GetAvailableStaffs(int bookingId)
        {
            var staffs = await _scheduleService.GetAvailableStaffForBooking(bookingId);
            return Ok(staffs);
        }

        [HttpGet("nullCollector")]
        public async Task<ActionResult<List<SampleCollectionScheduleDTO>>> GetAllNullCollector()
        {
            try
            {
                var list = await _scheduleService.GetAllWhereCollectorIsNullAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SampleCollectionScheduleDTO>> GetScheduleById(int id)
        {
            try
            {
                var schedule = await _scheduleService.GetSampleCollectionScheduleByIdAsync(id);
                if (schedule == null)
                    return NotFound($"Schedule with ID {id} not found");
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<ActionResult<SampleCollectionScheduleDTO>> GetScheduleByBookingId(int bookingId)
        {
            try
            {
                var schedule = await _scheduleService.GetScheduleByBookingIdAsync(bookingId);
                if (schedule == null)
                    return NotFound($"Schedule for booking ID {bookingId} not found");
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("collector/{collectorId}")]
        public async Task<ActionResult<List<SampleCollectionScheduleDTO>>> GetSchedulesByCollectorId(int collectorId)
        {
            try
            {
                var schedules = await _scheduleService.GetSchedulesByCollectorIdAsync(collectorId);
                return Ok(schedules);
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

                var result = await _scheduleService.CreateSampleCollectionScheduleAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateSchedule([FromBody] SampleCollectionScheduleDTO scheduleDto)
        {
            try
            {
                var result = await _scheduleService.UpdateSampleCollectionScheduleAsync(scheduleDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("AssignTask/{id}/{idStaff}")]
        public async Task<ActionResult<bool>> UpdateScheduleAssignTask(int id, int idStaff)
        {
            try
            {
                var result = await _scheduleService.UpdateScheduleAssignTaskAsync(id, idStaff);
                if (!result)
                    return NotFound($"Schedule with ID {id} not found");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteSchedule(int id)
        {
            try
            {
                var result = await _scheduleService.DeleteSampleCollectionScheduleAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<bool>> UpdateScheduleStatus(int id, [FromBody] string status)
        {
            try
            {
                var result = await _scheduleService.UpdateScheduleStatusAsync(id, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
