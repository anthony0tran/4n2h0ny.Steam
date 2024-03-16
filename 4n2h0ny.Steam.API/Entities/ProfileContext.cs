using Microsoft.EntityFrameworkCore;

namespace _4n2h0ny.Steam.API.Entities
{
    public class ProfileContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }

        public string DbPath { get; set; }
        public string DbPassword { get; set; }

        public ProfileContext(DbContextOptions<ProfileContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "4n2h0ny.Steam", "4n2h0ny.db");
            DbPassword = "Nichola!420"; // Replace password with value from secret manager
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={DbPath}; Password={DbPassword};");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Profile>()
                .HasIndex(p => p.URI)
                .IsUnique();

            builder.Entity<Profile>()
                .HasOne(p => p.ProfileData)
                .WithOne()
                .HasForeignKey<Profile>(p => p.ProfileDataId);

            builder.Entity<Profile>()
                .Navigation(p => p.ProfileData)
                .AutoInclude();

            builder.Entity<Profile>().HasQueryFilter(p => !p.IsExcluded);
            builder.Entity<Profile>().HasQueryFilter(p => !p.ProfileData.CommentAreaDisabled);
            builder.Entity<Profile>().HasQueryFilter(p => !p.ProfileNotFound);
        }
    }
}
