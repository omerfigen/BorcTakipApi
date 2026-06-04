using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BorcTakipApi.Data;
using BorcTakipApi.Models;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace BorcTakipApi.Controllers
{
    [ApiController] 
    [Route("api/borc")]
    public class BorcController : ControllerBase 
    {
        private readonly BorcDbContext _db;
        
        public BorcController(BorcDbContext db) 
        {
            _db = db;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Getir(int userId) 
        {
            var borclar = await _db.Borclar
                                   .Where(x => x.UserId == userId)
                                   .OrderByDescending(x => x.Tarih)
                                   .ToListAsync();
            return Ok(borclar);
        }

        [HttpPost]
        public async Task<IActionResult> Ekle([FromBody] Borc yeni) 
        {
            yeni.Tarih = DateTime.Now; 
            _db.Borclar.Add(yeni);
            await _db.SaveChangesAsync();
            return Ok(yeni);
        }

        [HttpPost("toggle/{id}")]
        public async Task<IActionResult> Toggle(int id) 
        {
            var b = await _db.Borclar.FindAsync(id);
            if (b != null) 
            { 
                b.IsPaid = !b.IsPaid; 
                await _db.SaveChangesAsync(); 
            }
            return Ok();
        }
    }
}
