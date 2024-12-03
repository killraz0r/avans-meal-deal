using AvansMealDeal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AvansMealDeal.Infrastructure.Identity.SQLServer
{
    // identity database
    public class DbContextIdentitySqlServer(DbContextOptions<DbContextIdentitySqlServer> options) : IdentityDbContext<MealDealUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // add roles
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            { 
                Name = Role.Student,
                NormalizedName = Role.Student.ToUpper()
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = Role.Employee,
                NormalizedName = Role.Employee.ToUpper()
            });
        }
    }
}
