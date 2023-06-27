using APILearning.Entities;
using Microsoft.EntityFrameworkCore;

namespace APILearning.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions options) : base (options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        public DbSet<AppUser> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
