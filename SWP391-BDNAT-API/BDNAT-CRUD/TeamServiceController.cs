using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace SWP391_BDNAT_API.Controllers
{
    // Controller cho bảng Team_Service
    public class TeamServiceController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TeamServiceController(AppDbContext context) => _context = context;

        // GET: api/teamservice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamService>>> Get() => await _context.TeamServices.ToListAsync();

        // GET: api/teamservice/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamService>> Get(int id)
        {
            var item = await _context.TeamServices.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        // POST: api/teamservice
        [HttpPost]
        public async Task<ActionResult<TeamService>> Post(TeamService item)
        {
            _context.TeamServices.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = item.TeamServiceID }, item);
        }

        // PUT: api/teamservice/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TeamService item)
        {
            if (id != item.TeamServiceID) return BadRequest();
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/teamservice/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.TeamServices.FindAsync(id);
            if (item == null) return NotFound();
            _context.TeamServices.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}