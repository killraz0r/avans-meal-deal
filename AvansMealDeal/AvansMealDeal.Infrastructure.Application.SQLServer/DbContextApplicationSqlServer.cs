using AvansMealDeal.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AvansMealDeal.Infrastructure.Application.SQLServer
{
    // application database
    public class DbContextApplicationSqlServer(DbContextOptions<DbContextApplicationSqlServer> options) : DbContext(options)
    {
        // tables
        public required DbSet<Canteen> Canteens { get; set; }
        public required DbSet<Meal> Meals { get; set; }
        public required DbSet<MealPackage> MealsPackages { get; set; }
        public required DbSet<MealPackageItem> MealPackageItems { get; set; }

        public required DbSet<Reservation> Reservations { get; set; }

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

            // add canteens (enumeration)
            modelBuilder.Entity<Canteen>().HasData(new Canteen
            {
                Id = 1,
                City = City.Breda,
                Address = "Hogeschoollaan 1",
                OffersHotMeals = true
            });
            modelBuilder.Entity<Canteen>().HasData(new Canteen
            {
                Id = 2,
                City = City.Breda,
                Address = "Lovensdijkstraat 61",
                OffersHotMeals = false
            });

            modelBuilder.Entity<Canteen>().HasData(new Canteen
            {
                Id = 3,
                City = City.Tilburg,
                Address = "Professor Cobbenhagenlaan 1",
                OffersHotMeals = true
            });
            modelBuilder.Entity<Canteen>().HasData(new Canteen
            {
                Id = 4,
                City = City.Tilburg,
                Address = "Professor Cobbenhagenlaan 13",
                OffersHotMeals = false
            });

            modelBuilder.Entity<Canteen>().HasData(new Canteen
            {
                Id = 5,
                City = City.DenBosch,
                Address = "Onderwijsboulevard 215",
                OffersHotMeals = true
            });
            modelBuilder.Entity<Canteen>().HasData(new Canteen
            {
                Id = 6,
                City = City.DenBosch,
                Address = "Parallelweg 23",
                OffersHotMeals = false
            });
        }
    }
}
