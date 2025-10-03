using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSet for Msg model
        public DbSet<Msg> Messages { get; set; }
        public DbSet<Police> Polices { get; set; }
        public DbSet<FIR> FIRs { get; set; }
        public DbSet<Thief> Thieves { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the composite key for the join table
            modelBuilder.Entity<FIRThief>().HasKey(ft => new { ft.FIRId, ft.ThiefId });

            // Configure the many-to-many relationship
            modelBuilder.Entity<FIRThief>()
                .HasOne(ft => ft.FIR)
                .WithMany(f => f.FIRThieves)
                .HasForeignKey(ft => ft.FIRId);

            modelBuilder.Entity<FIRThief>()
                .HasOne(ft => ft.Thief)
                .WithMany(t => t.FIRThieves)
                .HasForeignKey(ft => ft.ThiefId);
        }

    }
}
