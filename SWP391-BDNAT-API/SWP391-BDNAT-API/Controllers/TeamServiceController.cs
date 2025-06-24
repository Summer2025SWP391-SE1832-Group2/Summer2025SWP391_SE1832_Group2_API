//using System;
//using BDNAT_Repository.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace SWP391_BDNAT_API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TeamServiceController : ControllerBase
//    {
//        //private readonly AppDbContext _context;
//        //public TeamServiceController(AppDbContext context) => _context = context;

//        //[HttpGet] // Lấy toàn bộ team service
//        //public async Task<ActionResult<IEnumerable<TeamService>>> GetAll() => await _context.TeamServices.ToListAsync();

//        //[HttpGet("{id}")] // Lấy theo ID
//        //public async Task<ActionResult<TeamService>> GetById(int id)
//        //{
//        //    var entity = await _context.TeamServices.FindAsync(id);
//        //    return entity == null ? NotFound() : Ok(entity);
//        //}

//        //[HttpPost] // Tạo mới team service
//        //public async Task<ActionResult<TeamService>> Create(TeamService item)
//        //{
//        //    _context.TeamServices.Add(item);
//        //    await _context.SaveChangesAsync();
//        //    return CreatedAtAction(nameof(GetById), new { id = item.TeamServiceID }, item);
//        //}

//        //[HttpPut("{id}")] // Cập nhật
//        //public async Task<IActionResult> Update(int id, TeamService item)
//        //{
//        //    if (id != item.TeamServiceID) return BadRequest();
//        //    _context.Entry(item).State = EntityState.Modified;
//        //    await _context.SaveChangesAsync();
//        //    return NoContent();
//        //}

//        //[HttpDelete("{id}")] // Xoá
//        //public async Task<IActionResult> Delete(int id)
//        //{
//        //    var entity = await _context.TeamServices.FindAsync(id);
//        //    if (entity == null) return NotFound();
//        //    _context.TeamServices.Remove(entity);
//        //    await _context.SaveChangesAsync();
//        //    return NoContent();
//        }
//    }
//}
