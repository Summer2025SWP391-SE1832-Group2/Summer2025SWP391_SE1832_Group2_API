using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

// Controller cho bảng Team
public class TeamController : ControllerBase
{
    private readonly AppDbContext _context;
    public TeamController(AppDbContext context) => _context = context;

    // GET: api/team
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Team>>> Get() => await _context.Teams.ToListAsync();

    // GET: api/team/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Team>> Get(int id)
    {
        var item = await _context.Teams.FindAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    // POST: api/team
    [HttpPost]
    public async Task<ActionResult<Team>> Post(Team item)
    {
        _context.Teams.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = item.TeamID }, item);
    }

    // PUT: api/team/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Team item)
    {
        if (id != item.TeamID) return BadRequest();
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/team/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Teams.FindAsync(id);
        if (item == null) return NotFound();
        _context.Teams.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}