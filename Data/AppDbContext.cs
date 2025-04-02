using CinemaArchiveAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CinemaArchiveAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Director> Directors { get; set; }
    }
    
}

