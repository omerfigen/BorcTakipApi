using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class BorcController : ControllerBase
{
    private readonly BorcDbContext _context;

    public BorcController(BorcDbContext context)
    {
        _context = context;
    }

    // Listeleme: GET api/borc
    [HttpGet]
    public async Task<IActionResult> Getir() => Ok(await _context.Borclar.ToListAsync());

    // Ekleme: POST api/borc
    [HttpPost]
    public async Task<IActionResult> Ekle(Borc yeni)
    {
        _context.Borclar.Add(yeni);
        await _context.SaveChangesAsync();
        return Ok(yeni);
    }
}