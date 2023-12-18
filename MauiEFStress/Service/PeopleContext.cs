using MauiEFStress.Entity;
using Microsoft.EntityFrameworkCore;

namespace MauiEFStress.Service
{
    public class PeopleContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public PeopleContext()
        {
            SQLitePCL.Batteries_V2.Init();

            // Destoy and recreate the database for a clean restart
            //this.Database.EnsureDeleted();
            this.Database.EnsureCreated();

            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = true;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasKey(e => e.ID);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "People.db3");

            optionsBuilder.UseSqlite($"Filename={dbPath}");

            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
