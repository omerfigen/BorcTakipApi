using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BorcTakipApi.Data;
using BorcTakipApi.Models;
using System.Threading.Tasks;

namespace BorcTakipApi.Controllers
{
    [ApiController] 
    [Route("api/auth")]
    public class AuthController : ControllerBase 
    {
        private readonly BorcDbContext _db;
        
        public AuthController(BorcDbContext db) 
        {
            _db = db;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser) 
        {
            if (string.IsNullOrWhiteSpace(loginUser?.Username) || string.IsNullOrWhiteSpace(loginUser?.Password))
                return BadRequest(new { message = "Kullanıcı adı ve şifre boş olamaz" });
            
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == loginUser.Username && u.Password == loginUser.Password);
            return user == null ? Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı" }) : Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User newUser) 
        {
            if (string.IsNullOrWhiteSpace(newUser?.Username) || string.IsNullOrWhiteSpace(newUser?.Password))
                return BadRequest(new { message = "Kullanıcı adı ve şifre boş olamaz" });
            
            if (await _db.Users.AnyAsync(u => u.Username == newUser.Username)) 
                return BadRequest(new { message = "Bu kullanıcı adı zaten kullanılıyor" });
            
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();
            return Ok(newUser);
        }
    }
}
