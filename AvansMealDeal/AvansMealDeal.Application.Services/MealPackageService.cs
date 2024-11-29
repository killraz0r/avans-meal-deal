using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;

namespace AvansMealDeal.Application.Services
{
    public class MealPackageService : IMealPackageService
    {
        private readonly IMealPackageRepository mealPackageRepository;

        public MealPackageService(IMealPackageRepository mealPackageRepository)
        {
            this.mealPackageRepository = mealPackageRepository;
        }

		public async Task Add(MealPackage mealPackage, ICollection<int> mealIds)
		{
			await mealPackageRepository.Create(mealPackage);
            foreach (var mealId in mealIds) 
            {
                await mealPackageRepository.AddMealToPackage(mealPackage.Id, mealId);
            }
		}

		public async Task Edit(MealPackage mealPackage, ICollection<int> mealIds)
		{
            var mealPackageInDatabase = await GetById(mealPackage.Id);

            // null suppressed because meal package exists in the database
            mealPackageInDatabase!.Name = mealPackage.Name;
            mealPackageInDatabase.Price = mealPackage.Price;
            mealPackageInDatabase.MealPackageType = mealPackage.MealPackageType;
            mealPackageInDatabase.PickupDeadline = mealPackage.PickupDeadline;

			await mealPackageRepository.Update(mealPackageInDatabase);
            // replace the old list of meals with the new list of meals
			await mealPackageRepository.ClearMealsFromPackage(mealPackageInDatabase);
			foreach (var mealId in mealIds)
			{
				await mealPackageRepository.AddMealToPackage(mealPackage.Id, mealId);
			}
		}

        public async Task Remove(int id)
        {
			var mealPackage = await GetById(id);
			// null suppressed because meal package exists in the database
			await mealPackageRepository.Delete(mealPackage!);
		}

		public async Task<MealPackage?> GetById(int id)
        {
            return await mealPackageRepository.ReadById(id);
        }

        public async Task<ICollection<MealPackage>> GetForCanteen(int canteenId)
        {
            return await mealPackageRepository.ReadForCanteen(canteenId);
        }
    }
}
