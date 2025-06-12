//using System;
//using BDNAT_Repository.Entities;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace SWP391_BDNAT_API.Controllers
//{

//    [Route("api/[controller]")]
//    [ApiController]
//    public class TechnicalServiceController : ControllerBase
//    {
//        private readonly AppDbContext _context;
//        public TechnicalServiceController(AppDbContext context) => _context = context;

//        [HttpGet] // Lấy toàn bộ technical service
//        public async Task<ActionResult<IEnumerable<TechnicalService>>> GetAll() => await _context.TechnicalServices.ToListAsync();

//        [HttpGet("{id}")] // Lấy theo ID
//        public async Task<ActionResult<TechnicalService>> GetById(int id)
//        {
//            var entity = await _context.TechnicalServices.FindAsync(id);
//            return entity == null ? NotFound() : Ok(entity);
//        }

//        [HttpPost] // Tạo mới
//        public async Task<ActionResult<TechnicalService>> Create(TechnicalService item)
//        {
//            _context.TechnicalServices.Add(item);
//            await _context.SaveChangesAsync();
//            return CreatedAtAction(nameof(GetById), new { id = item.TechServiceID }, item);
//        }

//        [HttpPut("{id}")] // Cập nhật
//        public async Task<IActionResult> Update(int id, TechnicalService item)
//        {
//            if (id != item.TechServiceID) return BadRequest();
//            _context.Entry(item).State = EntityState.Modified;
//            await _context.SaveChangesAsync();
//            return NoContent();
//        }

//        [HttpDelete("{id}")] // Xoá
//        public async Task<IActionResult> Delete(int id)
//        {
//            var entity = await _context.TechnicalServices.FindAsync(id);
//            if (entity == null) return NotFound();
//            _context.TechnicalServices.Remove(entity);
//            await _context.SaveChangesAsync();
//            return NoContent();
//        }
//    }
//}
