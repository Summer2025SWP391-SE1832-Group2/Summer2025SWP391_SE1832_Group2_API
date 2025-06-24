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
    public class TeamController : Controller

    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }
        [HttpGet]
        public async Task<ActionResult<List<BlogDTO>>> GetAllTeamAsync()
        {
            try
            {
                //var blogs = await _blogService.GetAllBlogsAsync();
                var teams = await _teamService.GetAllTeamAsync();
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult<bool>> CreateTeam([FromBody] TeamDTO teamDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _teamService.CreateTeamAsync(teamDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDTO>> GetTeamById(int id)
        {
            try
            {
                var team = await _teamService.GetTeamByIdAsync(id);
                if (team == null)
                    return NotFound($"Blog with ID {id} not found");
                return Ok(team);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
