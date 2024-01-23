using Microsoft.EntityFrameworkCore;

namespace _4n2h0ny.Steam.API.Models
{
    public class ProfileContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        
        public string DbPath { get; }
        public string DbPassword { get; }

        public ProfileContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "4n2h0ny.Steam", "4n2h0ny.db");
            DbPassword = ""; // Replace password with value from secret manager
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}; Password={DbPassword};");
    }
}
