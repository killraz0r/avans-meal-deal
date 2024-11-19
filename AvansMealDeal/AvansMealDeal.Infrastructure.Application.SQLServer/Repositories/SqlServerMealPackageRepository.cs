﻿using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AvansMealDeal.Infrastructure.Application.SQLServer.Repositories
{
    public class SqlServerMealPackageRepository : IMealPackageRepository
    {
        private readonly DbContextApplicationSqlServer dbContext;

        public SqlServerMealPackageRepository(DbContextApplicationSqlServer dbContext) 
        {
            this.dbContext = dbContext;
        }

        public async Task Create(MealPackage mealPackage)
        {
            dbContext.MealsPackages.Add(mealPackage);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<MealPackage>> ReadForCity(City city)
        {
            return await dbContext.MealsPackages
                .Include(x => x.Canteen)
                .Include(x => x.Reservation)
                .Where(x => x.Canteen.City == city)
                .OrderBy(x => x.PickupDeadline)
                .ToListAsync();
        }

        public async Task<ICollection<MealPackage>> ReadForCanteen(int canteenId)
        {
            return await dbContext.MealsPackages
                .Include(x => x.Canteen)
                .Include(x => x.Reservation)
                .Where(x => x.Canteen.Id == canteenId)
                .OrderBy(x => x.PickupDeadline)
                .ToListAsync();
        }
    }
}
