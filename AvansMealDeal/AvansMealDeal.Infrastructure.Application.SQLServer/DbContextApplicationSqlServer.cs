using AvansMealDeal.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AvansMealDeal.Infrastructure.Application.SQLServer
{
    // application database
    public class DbContextApplicationSqlServer(DbContextOptions<DbContextApplicationSqlServer> options) : DbContext(options)
    {
        // tables
        public DbSet<Canteen> Canteens { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealPackage> MealsPackages { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // data types
            modelBuilder.Entity<MealPackage>()
                .Property(mp => mp.Price)
                .HasColumnType("decimal(18, 2)"); // euros

            // primary keys
            modelBuilder.Entity<Canteen>().HasKey(x => x.Id);
            modelBuilder.Entity<Meal>().HasKey(x => x.Id);
            modelBuilder.Entity<MealPackage>().HasKey(x => x.Id);
            modelBuilder.Entity<Reservation>().HasKey(x => x.Id);
            modelBuilder.Entity<MealPackageItem>().HasKey(x => new { x.MealId, x.MealPackageId }); // combination is the primary key

            // database relations
            modelBuilder.Entity<MealPackage>()
                .HasOne(x => x.Canteen)
                .WithMany(x => x.MealPackages)
                .HasForeignKey(x => x.CanteenId);

            modelBuilder.Entity<MealPackageItem>()
                .HasOne(x => x.Meal)
                .WithMany(x => x.MealPackages)
                .HasForeignKey(x => x.MealId);

            modelBuilder.Entity<MealPackageItem>()
                .HasOne(x => x.MealPackage)
                .WithMany(x => x.Meals)
                .HasForeignKey(x => x.MealPackageId);

            modelBuilder.Entity<MealPackage>()
                .HasOne(x => x.Reservation)
                .WithOne(x => x.MealPackage)
                .HasForeignKey<Reservation>(x => x.MealPackageId);
        }
    }
}
