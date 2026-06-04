using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BorcTakipApi.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BorcTakipApi.Controllers
{
    [ApiController] 
    [Route("api/stats")]
    public class StatsController : ControllerBase 
    {
        private readonly BorcDbContext _db;
        
        public StatsController(BorcDbContext db) 
        {
            _db = db;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetStats(int userId) 
        {
            var list = await _db.Borclar.Where(b => b.UserId == userId).ToListAsync();
            var borclarim = list.Where(x => x.Tip == "Alinan" && !x.IsPaid).Sum(x => x.Miktar);
            var alacaklarim = list.Where(x => x.Tip == "Verilen" && !x.IsPaid).Sum(x => x.Miktar);
            
            return Ok(new 
            { 
                totalAlinan = borclarim, 
                totalVerilen = alacaklarim, 
                netStatus = alacaklarim - borclarim 
            });
        }
    }
}
