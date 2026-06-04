using Microsoft.EntityFrameworkCore;
using BorcTakipApi.Models;

namespace BorcTakipApi.Data
{
    /// <summary>
    /// Veritabanı ile uygulama arasındaki köprüyü sağlayan Entity Framework context sınıfı.
    /// </summary>
    public class BorcDbContext : DbContext
    {
        public BorcDbContext(DbContextOptions<BorcDbContext> options) : base(options) 
        { 
        }

        /// <summary>
        /// Borçlar tablosuna erişim sağlar.
        /// </summary>
        public DbSet<Borc> Borclar => Set<Borc>();

        /// <summary>
        /// Kullanıcılar tablosuna erişim sağlar.
        /// </summary>
        public DbSet<User> Users => Set<User>();
    }
}
