using Microsoft.EntityFrameworkCore;

namespace _4n2h0ny.Steam.API.Models
{
    public class ProfileContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        
        public string DbPath { get; set; }
        public string DbPassword { get; set; }

        public ProfileContext(string? dbPath = null, string? dbPassword = null)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = dbPath ?? Path.Join(path, "4n2h0ny.Steam", "4n2h0ny.db");
            DbPassword = dbPassword ?? ""; // Replace password with value from secret manager
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}; Password={DbPassword};");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Profile>()
                .HasIndex(p => p.URI)
                .IsUnique();
        }
    }
}
