using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;

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

        public Task<ICollection<MealPackage>> ReadForLocation(City city)
        {
            throw new NotImplementedException();
        }
    }
}
