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

        public async Task Create(MealPackage mealPackage)
        {
            dbContext.MealsPackages.Add(mealPackage);
            await dbContext.SaveChangesAsync();
        }

		public async Task Update(MealPackage mealPackage)
        {
            dbContext.MealsPackages.Update(mealPackage);
			await dbContext.SaveChangesAsync();
		}

		public async Task ClearMealsFromPackage(MealPackage mealPackage)
        {
            foreach (var item in dbContext.MealPackageItems.Where(x => x.MealPackageId == mealPackage.Id))
            {
                dbContext.MealPackageItems.Remove(item);
            }
			await dbContext.SaveChangesAsync();
		}

		public async Task AddMealToPackage(int mealPackageId, int mealId)
        {
            dbContext.MealPackageItems.Add(new MealPackageItem
            {
                MealId = mealId,
                MealPackageId = mealPackageId
            });
			await dbContext.SaveChangesAsync();
		}

		public async Task Delete(MealPackage mealPackage)
        {
            dbContext.MealsPackages.Remove(mealPackage);
            await dbContext.SaveChangesAsync();
        }

		public async Task<MealPackage?> ReadById(int id)
        {
            return await dbContext.MealsPackages
                .Include(x => x.Canteen)
                .Include(x => x.Reservation)
                .Include(x => x.Meals).ThenInclude(x => x.Meal)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<MealPackage>> ReadForCanteen(int canteenId)
        {
            return await dbContext.MealsPackages
                .Include(x => x.Canteen)
                .Include(x => x.Reservation)
				.Include(x => x.Meals).ThenInclude(x => x.Meal)
				.Where(x => x.Canteen.Id == canteenId && x.PickupDeadline >= DateTimeOffset.Now)
                .OrderBy(x => x.PickupDeadline)
                .ToListAsync();
        }

        public async Task<ICollection<MealPackage>> ReadForOtherCanteens(int canteenId)
        {
            return await dbContext.MealsPackages
                .Include(x => x.Canteen)
                .Include(x => x.Reservation)
                .Include(x => x.Meals).ThenInclude(x => x.Meal)
                .Where(x => x.Canteen.Id != canteenId && x.PickupDeadline >= DateTimeOffset.Now)
                .OrderBy(x => x.PickupDeadline)
                .ToListAsync();
        }

        public async Task<ICollection<MealPackage>> ReadWithoutReservation()
        {
            return await dbContext.MealsPackages
                .Include(x => x.Canteen)
                .Include(x => x.Reservation)
                .Include(x => x.Meals).ThenInclude(x => x.Meal)
                .Where(x => x.Reservation == null && x.PickupDeadline >= DateTimeOffset.Now)
                .OrderBy(x => x.PickupDeadline)
                .ToListAsync();
        }

        public async Task<ICollection<MealPackage>> ReadWithReservationForStudent(string studentId)
        {
            return await dbContext.MealsPackages
                .Include(x => x.Canteen)
                .Include(x => x.Reservation)
                .Include(x => x.Meals).ThenInclude(x => x.Meal)
                .Where(x => x.Reservation != null && x.Reservation.StudentId == studentId)
                .OrderBy(x => x.PickupDeadline)
                .ToListAsync();
        }

        public IQueryable<MealPackage> Read() 
        { 
            return dbContext.MealsPackages.AsQueryable(); 
        }
    }
}
