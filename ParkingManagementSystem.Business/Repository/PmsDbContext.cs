using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ParkingManagementSystem.Business.Helper;
using ParkingManagementSystem.Models.Entities;
using ParkingManagementSystem.Models.Enums;

namespace ParkingManagementSystem.Business.Repository
{
    public class PmsDbContext : DbContext
    {
        public DbSet<ParkingEntity> ParkingEntities { get; set; }

        public PmsDbContext(DbContextOptions<PmsDbContext> options)
        : base(options)
        {
            Database.EnsureCreated();            
        }

        public void SeedDatabase()
        {
            if (!ParkingEntities.Any())
            {
                var dbSeedData = DbHelper.DBFeeder();
                ParkingEntities.AddRange(dbSeedData);
                SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkingEntity>()
                .ToTable("Parking")
                .HasKey(x => x.id);

            modelBuilder.Entity<ParkingEntity>()
                .Property(x => x.CarType)
                .HasConversion(new EnumToStringConverter<CarType>());

            modelBuilder.Entity<ParkingEntity>()
                .Property(x => x.ParkingStatus)
                .HasConversion(new EnumToStringConverter<ParkingStatus>());

            modelBuilder.Entity<ParkingEntity>()
                .Property(x => x.ParkingType)
                .HasConversion(new EnumToStringConverter<ParkingType>());
        }
    }
}
