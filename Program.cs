using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// SQL SERVER BAĞLANTISI
builder.Services.AddDbContext<BorcDbContext>(opt => 
    opt.UseSqlServer("Server=DESKTOP-LJR4TSD\\SQLEXPRESS;Database=PremiumBorcDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"));

builder.Services.AddCors(options => options.AddDefaultPolicy(p => 
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseCors();
app.MapControllers();

using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<BorcDbContext>();
   
    db.Database.EnsureCreated(); 
}

app.Run();


public class User {
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public class Borc {
    public int Id { get; set; }
    public string Isim { get; set; } = "";
    public double Miktar { get; set; }
    public string Aciklama { get; set; } = "";
    public string Tip { get; set; } = "Alinan"; 
    public int UserId { get; set; }             
    public DateTime Tarih { get; set; } = DateTime.Now;
    public bool IsPaid { get; set; } = false; 
}

public class BorcDbContext : DbContext {
    public BorcDbContext(DbContextOptions<BorcDbContext> options) : base(options) { }
    public DbSet<Borc> Borclar => Set<Borc>();
    public DbSet<User> Users => Set<User>();
}

//  API KONTROLLER
[ApiController] [Route("api/auth")]
public class AuthController : ControllerBase {
    private readonly BorcDbContext _db;
    public AuthController(BorcDbContext db) => _db = db;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User loginUser) {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == loginUser.Username && u.Password == loginUser.Password);
        return user == null ? Unauthorized() : Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User newUser) {
        if (await _db.Users.AnyAsync(u => u.Username == newUser.Username)) return BadRequest();
        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();
        return Ok(newUser);
    }
}

[ApiController] [Route("api/borc")]
public class BorcController : ControllerBase {
    private readonly BorcDbContext _db;
    public BorcController(BorcDbContext db) => _db = db;

    [HttpGet("{userId}")]
    public async Task<IActionResult> Getir(int userId) => 
        Ok(await _db.Borclar.Where(x => x.UserId == userId).OrderByDescending(x => x.Tarih).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Ekle([FromBody] Borc yeni) {
        yeni.Tarih = DateTime.Now; 
        _db.Borclar.Add(yeni);
        await _db.SaveChangesAsync();
        return Ok(yeni);
    }

    [HttpPost("toggle/{id}")]
    public async Task<IActionResult> Toggle(int id) {
        var b = await _db.Borclar.FindAsync(id);
        if (b != null) { b.IsPaid = !b.IsPaid; await _db.SaveChangesAsync(); }
        return Ok();
    }
}

[ApiController] [Route("api/stats")]
public class StatsController : ControllerBase {
    private readonly BorcDbContext _db;
    public StatsController(BorcDbContext db) => _db = db;
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetStats(int userId) {
        var list = await _db.Borclar.Where(b => b.UserId == userId).ToListAsync();
        var b = list.Where(x => x.Tip == "Alinan" && !x.IsPaid).Sum(x => x.Miktar);
        var a = list.Where(x => x.Tip == "Verilen" && !x.IsPaid).Sum(x => x.Miktar);
        return Ok(new { totalAlinan = b, totalVerilen = a, netStatus = a - b });
    }
}