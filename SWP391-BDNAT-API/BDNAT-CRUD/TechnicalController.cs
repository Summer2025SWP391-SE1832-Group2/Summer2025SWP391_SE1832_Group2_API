using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace SWP391_BDNAT_API.Controllers
{
    // Controller cho bảng Technical
    public class TechnicalController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TechnicalController(AppDbContext context) => _context = context;

        // GET: api/technical
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Technical>>> Get() => await _context.Technicals.ToListAsync();

        // GET: api/technical/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Technical>> Get(int id)
        {
            var item = await _context.Technicals.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        // POST: api/technical
        [HttpPost]
        public async Task<ActionResult<Technical>> Post(Technical item)
        {
            _context.Technicals.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = item.TechnicalID }, item);
        }

        // PUT: api/technical/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Technical item)
        {
            if (id != item.TechnicalID) return BadRequest();
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/technical/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Technicals.FindAsync(id);
            if (item == null) return NotFound();
            _context.Technicals.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
