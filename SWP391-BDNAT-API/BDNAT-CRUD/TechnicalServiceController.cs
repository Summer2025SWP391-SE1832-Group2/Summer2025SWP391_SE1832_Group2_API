using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace SWP391_BDNAT_API.Controllers
{

    // Controller cho bảng Technical_Service
    public class TechnicalServiceController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TechnicalServiceController(AppDbContext context) => _context = context;

        // GET: api/technicalservice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechnicalService>>> Get() => await _context.TechnicalServices.ToListAsync();

        // GET: api/technicalservice/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TechnicalService>> Get(int id)
        {
            var item = await _context.TechnicalServices.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        // POST: api/technicalservice
        [HttpPost]
        public async Task<ActionResult<TechnicalService>> Post(TechnicalService item)
        {
            _context.TechnicalServices.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = item.TechServiceID }, item);
        }

        // PUT: api/technicalservice/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TechnicalService item)
        {
            if (id != item.TechServiceID) return BadRequest();
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/technicalservice/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.TechnicalServices.FindAsync(id);
            if (item == null) return NotFound();
            _context.TechnicalServices.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}