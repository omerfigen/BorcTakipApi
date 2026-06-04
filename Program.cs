using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using BorcTakipApi.Data;
using System;
using System.Linq;

namespace BorcTakipApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. SERVİS KAYITLARI (DI Container)
            builder.Services.AddControllers();

            // Veritabanı bağlantısı yapılandırması
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrEmpty(connectionString))
            {
                builder.Services.AddDbContext<BorcDbContext>(opt => 
                    opt.UseSqlServer(connectionString));
            }
            else
            {
                // Fallback: SQLite
                builder.Services.AddDbContext<BorcDbContext>(opt => 
                    opt.UseSqlite("Data Source=borc.db"));
            }

            // CORS politikası
            builder.Services.AddCors(options => options.AddDefaultPolicy(p => 
                p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            var app = builder.Build();

            // 2. ARA KATMANLAR (MIDDLEWARE)
            app.UseDefaultFiles(); 
            app.UseStaticFiles();
            app.UseCors();
            app.MapControllers();

            Console.WriteLine("✓ Uygulama başlatılıyor...");

            
            InitializeDatabase(app);

            app.Run();
        }

        /// <summary>
        /// Uygulama başlatılırken veritabanını oluşturan ve gerekli temizlikleri yapan metod.
        /// </summary>
        private static void InitializeDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope()) 
            {
                try 
                {
                    var db = scope.ServiceProvider.GetRequiredService<BorcDbContext>();
                    
                    // Veritabanı ve tabloların var olduğundan emin ol
                    db.Database.EnsureCreated();
                    
                    // Boş kullanıcı kayıtlarını temizleme işlemi
                    var emptyUsers = db.Users
                        .Where(u => string.IsNullOrWhiteSpace(u.Username) || string.IsNullOrWhiteSpace(u.Password))
                        .ToList();
                        
                    if (emptyUsers.Any()) 
                    {
                        db.Users.RemoveRange(emptyUsers);
                        db.SaveChanges();
                    }
                    
                    Console.WriteLine("✓ Veritabanı hazır");
                } 
                catch (Exception ex) 
                {
                    Console.WriteLine($"✗ Veritabanı başlatma hatası: {ex.Message}");
                }
            }
        }
    }
}