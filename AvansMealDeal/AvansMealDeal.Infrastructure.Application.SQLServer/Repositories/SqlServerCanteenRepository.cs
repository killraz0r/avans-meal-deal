using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AvansMealDeal.Infrastructure.Application.SQLServer.Repositories
{
    public class SqlServerCanteenRepository : ICanteenRepository
    {
        private readonly DbContextApplicationSqlServer dbContext;

        public SqlServerCanteenRepository(DbContextApplicationSqlServer dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<Canteen>> ReadAll()
        {
            return await dbContext.Canteens.ToListAsync();
        }

        public async Task<Canteen> ReadById(int id)
        {
            return await dbContext.Canteens.SingleAsync(x => x.Id == id);
        }
    }
}
