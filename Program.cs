using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(); 

builder.Services.AddDbContext<BorcDbContext>(opt => opt.UseSqlite("Data Source=borclar.db"));

var app = builder.Build();


app.MapControllers(); 


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BorcDbContext>();
    db.Database.EnsureCreated();
}

app.Run();



public class Borc 
{
    public int Id { get; set; }
    public string Isim { get; set; } = "";
    public double Miktar { get; set; }
}

public class BorcDbContext : DbContext
{
    public BorcDbContext(DbContextOptions<BorcDbContext> options) : base(options) { }
    public DbSet<Borc> Borclar => Set<Borc>();
}