using AvansMealDeal.Domain.Models;
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

        public Task Create(MealPackage mealPackage)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<MealPackage>> ReadForCity(City city)
        {
            return await dbContext.MealsPackages
                .Include(x => x.Canteen)
                .Include(x => x.Reservation)
                .Where(x => x.Canteen.City == city)
                .ToListAsync();
        }

        public async Task<ICollection<MealPackage>> ReadForCanteen(int canteenId)
        {
            return await dbContext.MealsPackages
                .Include(x => x.Canteen)
                .Include(x => x.Reservation)
                .Where(x => x.Canteen.Id == canteenId)
                .ToListAsync();
        }
    }
}
