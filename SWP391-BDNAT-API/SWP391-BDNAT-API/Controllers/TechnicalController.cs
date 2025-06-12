using System;
using BDNAT_Repository.DTO;
using BDNAT_Repository.Entities;
using BDNAT_Service.Implementation;
using BDNAT_Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SWP391_BDNAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicalController : Controller

    {
        private readonly ITechnicalService _TechnicalService;

        public TechnicalController(ITechnicalService TechnicalService)
        {
            _TechnicalService = TechnicalService;
        }
        [HttpGet]
        public async Task<ActionResult<List<BlogDTO>>> GetAllTechnicalAsync()
        {
            try
            {
                //var blogs = await _blogService.GetAllBlogsAsync();
                var Technicals = await _TechnicalService.GetAllTechnicalAsync();
                return Ok(Technicals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult<bool>> CreateTechnical([FromBody] TechnicalDTO TechnicalDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _TechnicalService.CreateTechnicalAsync(TechnicalDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TechnicalDTO>> GetTechnicalById(int id)
        {
            try
            {
                var Technical = await _TechnicalService.GetTechnicalByIdAsync(id);
                if (Technical == null)
                    return NotFound($"Blog with ID {id} not found");
                return Ok(Technical);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
