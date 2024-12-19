using Microsoft.EntityFrameworkCore;
using gestion.Models;

namespace gestion.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Goods> Goods { get; set; }
        public DbSet<Users> Users { get; set; } 
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Review> Review { get; set; }
      
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Service;Username=postgres;Password=Paleo");
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
