using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AvansMealDeal.Infrastructure.Application.SQLServer.Repositories
{
    public class SqlServerMealRepository : IMealRepository
    {
        private readonly DbContextApplicationSqlServer dbContext;

        public SqlServerMealRepository(DbContextApplicationSqlServer dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Create(Meal meal)
        {
            dbContext.Meals.Add(meal);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Meal>> ReadAll()
        {
            return await dbContext.Meals.ToListAsync();
        }
    }
}
